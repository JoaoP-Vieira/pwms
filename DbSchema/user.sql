CREATE TABLE public."user" (
	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	email varchar NOT NULL,
	first_name varchar NOT NULL,
	last_name varchar NOT NULL,
	password_hash varchar NOT NULL,
	CONSTRAINT user_id PRIMARY KEY (id),
	CONSTRAINT user_email UNIQUE (email)
);
