<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.BuildStepViewModel>" %>
<span class="<%=Model.CssClass %>" title="<%: Model.StatusText %>">
    <%: Model.StepName %>
</span>