-- public.stock_item definição

-- Drop table

-- DROP TABLE public.stock_item;

CREATE TABLE public.stock_item (
	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	material_id uuid NOT NULL,
	label varchar(8) NOT NULL,
	location_id int4 NOT NULL,
	invoice_id uuid NOT NULL,
	invoice_line_number int4 NOT NULL,
	conference_user_id int4 NOT NULL,
	created_at timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	modified_at timestamp NULL,
	CONSTRAINT pk_stock_item PRIMARY KEY (id),
	CONSTRAINT uq_stock_item_label UNIQUE (label)
);

-- Índices para melhorar performance de consultas
CREATE INDEX idx_stock_item_material_id ON public.stock_item USING btree (material_id);
CREATE INDEX idx_stock_item_location_id ON public.stock_item USING btree (location_id);
CREATE INDEX idx_stock_item_invoice ON public.stock_item USING btree (invoice_id, invoice_line_number);
CREATE INDEX idx_stock_item_created_at ON public.stock_item USING btree (created_at);

-- public.stock_item chaves estrangeiras

ALTER TABLE public.stock_item ADD CONSTRAINT fk_stock_item_material FOREIGN KEY (material_id) REFERENCES public.material(id);
ALTER TABLE public.stock_item ADD CONSTRAINT fk_stock_item_location FOREIGN KEY (location_id) REFERENCES public.location(id);
ALTER TABLE public.stock_item ADD CONSTRAINT fk_stock_item_invoice FOREIGN KEY (invoice_id) REFERENCES public.invoice(id);
ALTER TABLE public.stock_item ADD CONSTRAINT fk_stock_item_invoice_item FOREIGN KEY (invoice_id, invoice_line_number) REFERENCES public.invoice_item(invoice_id, line_number);
ALTER TABLE public.stock_item ADD CONSTRAINT fk_stock_item_user FOREIGN KEY (conference_user_id) REFERENCES public."user"(id);
