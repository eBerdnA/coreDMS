CREATE TABLE `FileStates` ( 
	`id` INTEGER PRIMARY KEY AUTOINCREMENT, 
	`name` VARCHAR(255), 
	`createdAt` DATETIME NOT NULL, 
	`updatedAt` DATETIME NOT NULL 
	);

INSERT INTO `FileStates` (name, createdAt, updatedAt) VALUES ('new', strftime('%Y-%m-%dT%H:%M:%S',DATETIME('now', 'utc')), strftime('%Y-%m-%dT%H:%M:%S',DATETIME('now', 'utc')));