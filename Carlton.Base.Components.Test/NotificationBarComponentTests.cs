﻿namespace Carlton.Base.Components.Test;

public class NotificationBarComponentTests : TestContext
{
    private static readonly string NotificationBarMarkup =
     @"
<div class=""notification-bar"" b-5w4ezungf0>
    <div class=""content"" style=""top:50px;right:70px;"" b-5w4ezungf0></div>
</div>
<div class=""test-btns"">
    <button class=""success-btn"" blazor:onclick=""1"" b-5w4ezungf0>Success</button>
    <button class=""info-btn"" blazor:onclick=""2"" b-5w4ezungf0>Info</button>
    <button class=""warning-btn"" blazor:onclick=""3"" b-5w4ezungf0>Warning</button>
    <button class=""error-btn"" blazor:onclick=""4"" b-5w4ezungf0>Error</button>
</div>";

    private static readonly string TestBtnsMarkup =
    @"<div class=""test-btns"" b-5w4ezungf0>
        <button class=""success-btn"" blazor:onclick=""1"" b-5w4ezungf0>Success</button>
        <button class=""info-btn"" blazor:onclick=""2"" b-5w4ezungf0>Info</button>
        <button class=""warning-btn"" blazor:onclick=""3"" b-5w4ezungf0>Warning</button>
        <button class=""error-btn"" blazor:onclick=""4"" b-5w4ezungf0>Error</button>
    </div>";

    public static IEnumerable<object[]> GetNotifications()
    {
        yield return new object[]
           {
               new List<int> { 2, 7, 3, 4 }
           };
        yield return new object[]
           {
               new List<int> { 1, 2, 3, 4 }
           };
        yield return new object[]
            {
               new List<int> { 0, 1, 3, 0 }
            };
    }

    [Fact]
    public void NotificationBar_Markup_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, 50)
            .Add(p => p.Right, 70)
            );

        //Assert
        cut.MarkupMatches(NotificationBarMarkup);
    }

    [Fact]
    public void NotificationBar_IsTestModeFalseParam_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, false)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, 50)
            .Add(p => p.Right, 70)
            );

        //Assert
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".test-btns"));
    }

    [Fact]
    public void NotificationBar_IsTestModeTrueParam_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, 50)
            .Add(p => p.Right, 70)
            );

        var testBtns = cut.Find(".test-btns");

        //Assert
        testBtns.MarkupMatches(TestBtnsMarkup);
    }

    [Theory]
    [InlineData(true, "success-btn")]
    [InlineData(true, "info-btn")]
    [InlineData(true, "warning-btn")]
    [InlineData(true, "error-btn")]
    [InlineData(false, "success-btn")]
    [InlineData(false, "info-btn")]
    [InlineData(false, "warning-btn")]
    [InlineData(false, "error-btn")]
    public void NotificationBar_FadeOutEnabledParam_RendersCorrectly(bool fadeOutEnabled, string notificationClass)
    {
        //Arrange
        var moduleInterop = JSInterop.SetupModule("./_content/Carlton.Base.Components/Notifications/BaseNotification.razor.js");
        moduleInterop.Setup<Task>("applyTransitionedCallback");

        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, fadeOutEnabled)
            .Add(p => p.Top, 50)
            .Add(p => p.Right, 70)
            );

        cut.Find($".{notificationClass}").Click();

        var notification = cut.FindComponent<BaseNotification>().Instance;

        //Assert
        Assert.Equal(fadeOutEnabled, notification.FadeOutEnabled);
    }

    [Theory]
    [InlineData(5, 10, "top:5px;right:10px;")]
    [InlineData(10, 5, "top:10px;right:5px;")]
    [InlineData(7, 7, "top:7px;right:7px;")]
    public void NotificationBar_TopRightParams_RendersCorrectly(int top, int right, string expectedResult)
    {
        //Arrange
        var moduleInterop = JSInterop.SetupModule("./_content/Carlton.Base.Components/Notifications/BaseNotification.razor.js");
        moduleInterop.Setup<Task>("applyTransitionedCallback");

        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, top)
            .Add(p => p.Right, right)
            );

        var content = cut.Find(".content");
        var style = content?.Attributes?.GetNamedItem("style")?.TextContent;

        //Assert
        Assert.Equal(expectedResult, style);
    }

    [Theory]
    [InlineData("success-btn", typeof(SuccessNotification))]
    [InlineData("info-btn", typeof(InfoNotification))]
    [InlineData("warning-btn", typeof(WarningNotification))]
    [InlineData("error-btn", typeof(ErrorNotification))]
    public void NotificationBar_ClickEvent_RendersCorrectly(string btnClass, Type expectedType)
    {
        //Arrange
        var moduleInterop = JSInterop.SetupModule("./_content/Carlton.Base.Components/Notifications/BaseNotification.razor.js");
        moduleInterop.Setup<Task>("applyTransitionedCallback");

        //Act
        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, 10)
            .Add(p => p.Right, 10)
            );

        cut.Find($".{btnClass}").Click();
        var component = cut.FindComponent<BaseNotification>().Instance;

        //Assert
        Assert.Equal(expectedType, component.GetType());
    }

    [Theory]
    [MemberData(nameof(GetNotifications))]
    public void NotificationBar_ClickEventRepeatedly_RendersCorrectly(IEnumerable<int> clickCounts)
    {
        //Arrange
        var moduleInterop = JSInterop.SetupModule("./_content/Carlton.Base.Components/Notifications/BaseNotification.razor.js");
        moduleInterop.Setup<Task>("applyTransitionedCallback");

        var cut = RenderComponent<NotificationBar>(parameters => parameters
            .Add(p => p.IsTestMode, true)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.Top, 10)
            .Add(p => p.Right, 10)
            );

        var successBtn = cut.Find(".success-btn");
        var infoBtn = cut.Find(".info-btn");
        var warningBtn = cut.Find(".warning-btn");
        var errorBtn = cut.Find(".error-btn");

        //Act
        for(var i = 0; i < clickCounts.ElementAt(0); i++)
            successBtn.Click();
        for(var i = 0; i < clickCounts.ElementAt(1); i++)
            infoBtn.Click();
        for(var i = 0; i < clickCounts.ElementAt(2); i++)
            warningBtn.Click();
        for(var i = 0; i < clickCounts.ElementAt(3); i++)
            errorBtn.Click();


        var successActual = cut.FindComponents<SuccessNotification>().Count;
        var infoActual = cut.FindComponents<InfoNotification>().Count;
        var warnActual = cut.FindComponents<WarningNotification>().Count;
        var errorActual = cut.FindComponents<ErrorNotification>().Count();

        var successExpected = clickCounts.ElementAt(0);
        var infoExpected = clickCounts.ElementAt(1);
        var warnExpected = clickCounts.ElementAt(2);
        var errorExpected = clickCounts.ElementAt(3);

        //Assert
        Assert.Equal(successExpected, successActual);
        Assert.Equal(infoExpected, infoActual);
        Assert.Equal(warnExpected, warnActual);
        Assert.Equal(errorExpected, errorActual);
    }
}
