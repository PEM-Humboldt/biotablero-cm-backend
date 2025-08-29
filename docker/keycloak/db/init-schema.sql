-- Add permissions to public schema in keycloak database
\c keycloak

ALTER SCHEMA public OWNER TO keycloak_user;
GRANT ALL ON SCHEMA public TO keycloak_user;
