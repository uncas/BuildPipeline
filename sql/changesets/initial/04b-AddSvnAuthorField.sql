UPDATE Pipeline
SET SourceAuthor = 'ole'


ALTER TABLE Pipeline
    ALTER COLUMN
        SourceAuthor    nvarchar(64)    NOT NULL