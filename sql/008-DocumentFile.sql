CREATE TABLE "DocumentFiles" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "title" VARCHAR(255) NOT NULL DEFAULT "document file",
    "note" text,
    "createdAt" DATETIME NOT NULL, 
	"updatedAt" DATETIME NOT NULL
)