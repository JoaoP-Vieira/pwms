-- public.material definição

-- Drop table

-- DROP TABLE public.material;

CREATE TABLE public.material (
	id uuid NOT NULL,
	sku varchar NOT NULL,
	barcode varchar NOT NULL,
	"name" varchar NOT NULL,
	description varchar NOT NULL,
	category_id int4 NULL,
	weight numeric NOT NULL,
	height numeric NOT NULL,
	width numeric NOT NULL,
	length numeric NOT NULL,
	status int4 NOT NULL,
	created_at timestamp NOT NULL,
	updated_at timestamp NULL,
	CONSTRAINT material_barcode UNIQUE (barcode),
	CONSTRAINT material_id PRIMARY KEY (id),
	CONSTRAINT material_sku UNIQUE (sku)
);


-- public.material chaves estrangeiras

ALTER TABLE public.material ADD CONSTRAINT material_category_fk FOREIGN KEY (category_id) REFERENCES public.category(id) ON DELETE SET NULL;