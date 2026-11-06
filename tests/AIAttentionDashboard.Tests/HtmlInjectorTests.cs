namespace AIAttentionDashboard.Tests;

public class HtmlInjectorTests
{
    [Fact]
    public Task InsertsScriptBeforeClosingBody() =>
        Verify(HtmlInjector.Inject(DashboardFiles.StubHtml), "html");

    [Fact]
    public Task AppendsScriptWhenThereIsNoBody() =>
        Verify(HtmlInjector.Inject("<p>hi</p>"), "html");
}
