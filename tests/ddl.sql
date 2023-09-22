CREATE UNLOGGED TABLE "Pessoas" (
    "Id" uuid NOT NULL CONSTRAINT "PK_Pessoas" PRIMARY KEY,
    "Apelido" character varying(32) NOT NULL,
    "Nome" character varying(100) NOT NULL,
    "Nascimento" date NOT NULL,
    "Stack" character varying(32)[],
    "Busca" TEXT
);

CREATE EXTENSION IF NOT EXISTS PG_TRGM;
CREATE INDEX CONCURRENTLY IF NOT EXISTS IDX_PESSOAS_BUSCA ON "Pessoas" USING GIST ("Busca" GIST_TRGM_OPS(SIGLEN=64));
