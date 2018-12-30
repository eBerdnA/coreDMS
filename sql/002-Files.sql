CREATE TABLE "Files" ( 
	"id" VARCHAR(36) PRIMARY KEY, 
	"filename" VARCHAR(255), 
	"hash" VARCHAR(32) NOT NULL UNIQUE, 
	"document_date" DATETIME, 
	"state" INTEGER NOT NULL DEFAULT 0, 
	"location" VARCHAR(255) NOT NULL DEFAULT "", 
	"createdAt" DATETIME NOT NULL, 
	"updatedAt" DATETIME NOT NULL , 
	title VARCHAR(255) NOT NULL DEFAULT "filename"
	)