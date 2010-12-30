ALTER TABLE Pipeline
    ADD
        SourceAuthor    nvarchar(64)    NULL

GO

UPDATE Pipeline
SET SourceAuthor = 'ole'

GO

ALTER TABLE Pipeline
    ALTER COLUMN
        SourceAuthor    nvarchar(64)    NOT NULL