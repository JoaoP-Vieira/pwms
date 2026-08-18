-- public.invoice definição

-- Drop table

-- DROP TABLE public.invoice;

CREATE TABLE public.invoice (
	id uuid NOT NULL,
	invoice_number varchar NOT NULL,
	series varchar NOT NULL,
	verification_code varchar NOT NULL,
	invoice_type int4 NOT NULL,
	status int4 NOT NULL,
	issuer_id int4 NOT NULL,
	recipient_id int4 NOT NULL,
	carrier_id int4 NULL,
	issue_date timestamp NOT NULL,
	expected_delivery_date timestamp NULL,
	total_amount numeric NOT NULL,
	total_volumes int4 NOT NULL,
	created_at timestamp NOT NULL,
	modified_at timestamp NULL,
	conferency_location_id int4 NULL,
	plate_num_veh varchar NULL,
	CONSTRAINT invoice_id PRIMARY KEY (id),
	CONSTRAINT invoice_number UNIQUE (invoice_number),
	CONSTRAINT invoice_number_series UNIQUE (invoice_number, series)
);


-- public.invoice chaves estrangeiras

ALTER TABLE public.invoice ADD CONSTRAINT fk_invoice_carrier_id FOREIGN KEY (carrier_id) REFERENCES public.person(id);
ALTER TABLE public.invoice ADD CONSTRAINT fk_invoice_issuer_id FOREIGN KEY (issuer_id) REFERENCES public.person(id);
ALTER TABLE public.invoice ADD CONSTRAINT fk_invoice_recipient_id FOREIGN KEY (recipient_id) REFERENCES public.person(id);
ALTER TABLE public.invoice ADD CONSTRAINT invoice_location_fk FOREIGN KEY (conferency_location_id) REFERENCES public."location"(id) ON DELETE SET NULL;