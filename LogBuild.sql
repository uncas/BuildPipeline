EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/branches/Experiment1'
    , @SourceRevision = $(SourceRevision)
    , @IsSuccessFul = $(IsSuccessful)
    , @StepName = '$(StepName)'
    , @BuildNumber = $(BuildNumber)