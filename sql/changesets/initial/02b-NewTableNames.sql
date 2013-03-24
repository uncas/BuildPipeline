UPDATE BuildStep
SET PipelineId = BuildId


ALTER TABLE BuildStep
    ALTER
        COLUMN PipelineId    int    NOT NULL


ALTER TABLE BuildStep
    ALTER
        COLUMN BuildId    int    NULL