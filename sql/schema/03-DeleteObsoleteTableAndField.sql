ALTER TABLE BuildStep
    DROP
        CONSTRAINT FK_BuildStep_Build

GO

ALTER TABLE BuildStep
    DROP
        COLUMN BuildId

GO

DROP TABLE Build