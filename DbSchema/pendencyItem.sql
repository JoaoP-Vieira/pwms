-- public.pendency_item definição

-- Drop table

-- DROP TABLE public.pendency_item;

CREATE TABLE public.pendency_item (
	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	material_id uuid NOT NULL,
	pendency_type int4 NOT NULL,
	description varchar(500) NOT NULL,
	invoice_id uuid NOT NULL,
	invoice_line_number int4 NOT NULL,
	location_id int4 NULL,
	status int4 NOT NULL DEFAULT 0,
	created_by_user_id int4 NOT NULL,
	resolved_by_user_id int4 NULL,
	created_at timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	resolved_at timestamp NULL,
	CONSTRAINT pk_pendency_item PRIMARY KEY (id)
);

-- Índices para melhorar performance de consultas
CREATE INDEX idx_pendency_item_material_id ON public.pendency_item USING btree (material_id);
CREATE INDEX idx_pendency_item_invoice ON public.pendency_item USING btree (invoice_id, invoice_line_number);
CREATE INDEX idx_pendency_item_status ON public.pendency_item USING btree (status);
CREATE INDEX idx_pendency_item_type ON public.pendency_item USING btree (pendency_type);
CREATE INDEX idx_pendency_item_created_at ON public.pendency_item USING btree (created_at);
CREATE INDEX idx_pendency_item_created_by ON public.pendency_item USING btree (created_by_user_id);

-- public.pendency_item chaves estrangeiras

ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_material FOREIGN KEY (material_id) REFERENCES public.material(id);
ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_invoice FOREIGN KEY (invoice_id) REFERENCES public.invoice(id);
ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_invoice_item FOREIGN KEY (invoice_id, invoice_line_number) REFERENCES public.invoice_item(invoice_id, line_number);
ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_location FOREIGN KEY (location_id) REFERENCES public.location(id) ON DELETE SET NULL;
ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_created_by_user FOREIGN KEY (created_by_user_id) REFERENCES public."user"(id);
ALTER TABLE public.pendency_item ADD CONSTRAINT fk_pendency_item_resolved_by_user FOREIGN KEY (resolved_by_user_id) REFERENCES public."user"(id) ON DELETE SET NULL;
