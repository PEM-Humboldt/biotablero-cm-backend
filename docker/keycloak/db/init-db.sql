-- Create a dummy role in postgres (for delete some messages in the logs)
CREATE ROLE postgres LOGIN;

-- Create keycloak database
CREATE DATABASE keycloak;
CREATE USER keycloak_user WITH ENCRYPTED PASSWORD 'keycloak_pass';
GRANT ALL PRIVILEGES ON DATABASE keycloak TO keycloak_user;
