CREATE TABLE "DocumentFileFile" ( 
	"id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
	"DocumentFileId" INTEGER NOT NULL , 
	"FileId" VARCHAR(36) NOT NULL, 
	`createdAt` DATETIME NOT NULL, 
	`updatedAt` DATETIME NOT NULL, 
	FOREIGN KEY (DocumentFileId) REFERENCES DocumentFiles(id), 
	FOREIGN KEY (FileId) REFERENCES Files(id) 
)