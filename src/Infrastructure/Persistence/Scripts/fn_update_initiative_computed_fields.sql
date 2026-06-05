-- Main function for calculating the fields
CREATE OR REPLACE FUNCTION initiatives.fn_update_initiative_computed_fields()
RETURNS TRIGGER AS $$
DECLARE
    v_initiative_id INT;
    v_geom GEOMETRY;
    v_coordinate GEOMETRY := ST_GeomFromText('POINT EMPTY', 4326);
    v_area_ha DOUBLE PRECISION := 0;
    v_main_location_id INT := 0;
    v_save_area BOOLEAN := TRUE;
BEGIN
    -- 1. Determine the initiative ID depending on which table triggered the event.
    IF TG_TABLE_NAME = 'initiative_location' THEN
        IF TG_OP = 'DELETE' THEN
            v_initiative_id := OLD.initiative_id;
        ELSE
            v_initiative_id := NEW.initiative_id;
        END IF;
        
        -- If the relationship has changed, read the current polygon directly from the initiative in case it exists
        SELECT polygon INTO v_geom FROM initiatives.initiative WHERE id = v_initiative_id;
        
    ELSIF TG_TABLE_NAME = 'initiative' THEN
        v_initiative_id := NEW.id;
        v_geom := NEW.polygon;
    END IF;

    -- 2. If the initiative does not have a direct polygon, calculate from the associated locations
    IF v_geom IS NULL THEN
        -- Try merging associated municipalities (level = 3)
        SELECT ST_Union(lp.geometry) INTO v_geom
        FROM initiatives.initiative_location il
        JOIN geo.location l ON il.location_id = l.id
        JOIN geo.location_polygon lp ON l.id = lp.location_id
        WHERE il.initiative_id = v_initiative_id AND l.level = 3;

        -- If you have no municipalities, try merging associated departments (level = 2)
        IF v_geom IS NULL THEN
            v_save_area := FALSE;
            SELECT ST_Union(lp.geometry) INTO v_geom
            FROM initiatives.initiative_location il
            JOIN geo.location l ON il.location_id = l.id
            JOIN geo.location_polygon lp ON l.id = lp.location_id
            WHERE il.initiative_id = v_initiative_id AND l.level = 2;
        END IF;
    END IF;

    -- 3. Calculate Centroid, Area and MainLocation
    IF v_geom IS NOT NULL THEN
        -- Calculate Centroid
        v_coordinate := ST_Centroid(v_geom);
        
        -- Calculate area in hectares. Casting to ::geography gives us an exact calculation in square meters. / 10000 = Hectares
        IF v_save_area IS TRUE THEN
            v_area_ha := ST_Area(v_geom::geography) / 10000.0;
        END IF;
        
        -- Find the parent department (level = 2) that intersects with the new centroid
        SELECT l.id INTO v_main_location_id
        FROM geo.location l
        JOIN geo.location_polygon lp ON l.id = lp.location_id
        WHERE l.level = 2 AND ST_Intersects(lp.geometry, v_coordinate)
        LIMIT 1;
    ELSE
        -- Default values ​​if no geographical reference exists
        v_coordinate := ST_GeomFromText('POINT EMPTY', 4326);
        v_area_ha := 0;
        v_main_location_id := 0;
    END IF;

    -- 4. Apply the changes
    IF TG_TABLE_NAME = 'initiative_location' THEN
        -- Trigger invoked from the child table -> We update the parent table
        UPDATE initiatives.initiative
        SET coordinate = v_coordinate,
            polygon_area = v_area_ha,
            main_location_id = v_main_location_id
        WHERE id = v_initiative_id;
        
        RETURN NULL; -- AFTER triggers must return NULL or NEW, it's irrelevant
    ELSE
        -- Trigger invoked from the main table -> We modify the ROW in memory (BEFORE UPDATE/INSERT)
        NEW.coordinate := v_coordinate;
        NEW.polygon_area := v_area_ha;
        NEW.main_location_id := v_main_location_id;
        RETURN NEW;
    END IF;

END;
$$ LANGUAGE plpgsql;

-- Create the trigger for when an Initiative-Location relationship is added, deleted, or edited
DROP TRIGGER IF EXISTS trg_initiative_location_update ON initiatives.initiative_location;
CREATE TRIGGER trg_initiative_location_update
AFTER INSERT OR UPDATE OR DELETE ON initiatives.initiative_location
FOR EACH ROW EXECUTE FUNCTION initiatives.fn_update_initiative_computed_fields();

-- Create the Trigger for when the polygon of an Initiative is updated (BEFORE prevents infinite loops)
DROP TRIGGER IF EXISTS trg_initiative_compute_fields ON initiatives.initiative;
CREATE TRIGGER trg_initiative_compute_fields
BEFORE INSERT OR UPDATE OF polygon ON initiatives.initiative
FOR EACH ROW EXECUTE FUNCTION initiatives.fn_update_initiative_computed_fields();