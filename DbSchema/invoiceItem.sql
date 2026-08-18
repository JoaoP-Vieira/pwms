-- public.invoice_item definição

-- Drop table

-- DROP TABLE public.invoice_item;

CREATE TABLE public.invoice_item (
	invoice_id uuid NOT NULL,
	line_number int4 NOT NULL,
	declared_quantity numeric NOT NULL,
	processed_quantity numeric NULL,
	unity_price numeric NOT NULL,
	material_id uuid NOT NULL,
	conference_user_id int4 NULL,
	conference_date timestamp NULL,
	CONSTRAINT ci_invoice_id_line_number UNIQUE (invoice_id, line_number)
);


-- public.invoice_item chaves estrangeiras

ALTER TABLE public.invoice_item ADD CONSTRAINT fk_invoice_item_invoice_id FOREIGN KEY (invoice_id) REFERENCES public.invoice(id);
ALTER TABLE public.invoice_item ADD CONSTRAINT invoice_item_material_fk FOREIGN KEY (material_id) REFERENCES public.material(id);
ALTER TABLE public.invoice_item ADD CONSTRAINT invoice_item_user_fk FOREIGN KEY (conference_user_id) REFERENCES public."user"(id) ON DELETE SET NULL;