CREATE TABLE BuildStep
(
    BuildStepId    integer    NOT NULL
        PRIMARY KEY ASC
    , IsSuccessful    integer    NOT NULL
    , StepName    text    NOT NULL
    , BuildNumber    integer    NOT NULL
    , Created    datetime    NOT NULL
    , PipelineId    int    NOT NULL
    , FOREIGN KEY (PipelineId) REFERENCES Pipeline (PipelineId)
)
