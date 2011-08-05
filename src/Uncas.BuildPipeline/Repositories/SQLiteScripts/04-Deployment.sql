CREATE TABLE Deployment
(
    DeploymentId    integer    NOT NULL
        PRIMARY KEY ASC
    , Created    datetime    NOT NULL
    , PipelineId    integer    NOT NULL
    , EnvironmentId    integer    NOT NULL
    , Started    datetime    NULL
    , Completed    datetime    NULL
    , FOREIGN KEY (PipelineId) REFERENCES Pipeline (PipelineId)
 )