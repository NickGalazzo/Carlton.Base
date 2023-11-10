﻿using AutoFixture.Xunit2;
using static Carlton.Core.Components.Library.Tests.TableTestHelper;

namespace Carlton.Core.Components.Library.Tests;

[Trait("Component", nameof(TableHeader<int>))]
public class TableHeaderComponentTests : TestContext
{
    [Theory(DisplayName = "Markup Test"), AutoData]
    public void TableHeader_Markup_RendersCorrectly(IEnumerable<TableHeadingItem> headings)
    {
        //Arrange
        var expected = BuildExpectedHeaderMarkup(headings);

        //Act
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true));

        //Assert
        cut.MarkupMatches(expected);
    }

    [Theory(DisplayName = "Headings Parameter Test"), AutoData]
    public void TableHeader_HeadingsParam_RendersCorrectly(IEnumerable<TableHeadingItem> headings)
    {
        //Arrange
        var expectedCount = headings.Count();
        var expectedHeadings = headings.Select(_ => _.DisplayName);

        //Act
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true));

        var headerRowItems = cut.FindAll(".header-cell");
        var actualCount = headerRowItems.Count;
        var actualHeadings = cut.FindAll(".heading-text").Select(_ => _.TextContent);

        //Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedHeadings, actualHeadings);
    }

    [Theory(DisplayName = "OrderColumn and OrderDirection Parameter Test")]
    [InlineData("ID", 0, true)]
    [InlineData("ID", 0, false)]
    [InlineData("DisplayName", 1, true)]
    [InlineData("DisplayName", 1, false)]
    [InlineData("CreatedDate", 2, true)]
    [InlineData("CreatedDate", 2, false)]
    public void TableHeader_OrderColumnParam_And_OrderDirectionParam_RendersCorrectly(string columnName, int columnIndex, bool orderAscending)
    {
        //Act
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, columnName)
            .Add(p => p.OrderAscending, orderAscending));

        var headerRowItems = cut.FindAll(".header-cell");
        var selectedItem = headerRowItems.ElementAt(columnIndex);
        var hasSelectedClass = selectedItem.ClassList.Contains("selected");
        var hasAscendingClass = selectedItem.ClassList.Contains("ascending");
        var hasDescendingClass = selectedItem.ClassList.Contains("descending");

        //Assert
        Assert.True(hasSelectedClass);
        Assert.Equal(orderAscending, hasAscendingClass);
        Assert.Equal(orderAscending, !hasDescendingClass);
    }

    [Theory(DisplayName = "Invalid OrderColumn Parameter Test")]
    [InlineData("Wrong")]
    [InlineData("Also Wrong")]
    [InlineData("Still Wrong")]
    public void TableHeader_InvalidOrderColumnParam_RendersCorrectly(string columnName)
    {
        //Arrange
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, columnName)
            .Add(p => p.OrderAscending, true));

        //Act
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".selected"));
    }

    [Theory(DisplayName = "Header Click Once Test")]
    [InlineData(0, "ID")]
    [InlineData(1, "DisplayName")]
    [InlineData(2, "CreatedDate")]
    public void TableHeader_ClickHeadersOnce_EventFires(int columnIndex, string expectedColumnName)
    {
        //Arrange
        var eventFired = false;
        var actualOrderAscending = false;
        var actualOrderColumn = string.Empty;
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true)
            .Add(p => p.OnItemsOrdered, args => { eventFired = true; actualOrderAscending = args.OrderAscending; actualOrderColumn = args.OrderColumn; }));

        var headerRowItems = cut.FindAll(".header-cell");

        //Act
        headerRowItems.ElementAt(columnIndex).Click();

        //Assert
        Assert.True(eventFired);
        Assert.True(actualOrderAscending);
        Assert.Equal(expectedColumnName, actualOrderColumn);
    }

    [Theory(DisplayName = "Header Click Twice Test")]
    [InlineData(0, "ID")]
    [InlineData(1, "DisplayName")]
    [InlineData(2, "CreatedDate")]
    public void TableHeader_ClickHeadersTwice_EventFires(int columnIndex, string expectedColumnName)
    {
        //Arrange
        var eventFired = false;
        var actualOrderAscending = true;
        var actualOrderColumn = string.Empty;
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true)
            .Add(p => p.OnItemsOrdered, args => { eventFired = true; actualOrderAscending = args.OrderAscending; actualOrderColumn = args.OrderColumn; }));

        //Act
        var itemToClick = cut.FindAll(".header-cell").ElementAt(columnIndex);
        itemToClick.Click();
        itemToClick = cut.FindAll(".header-cell").ElementAt(columnIndex);
        itemToClick.Click();

        //Assert
        Assert.True(eventFired);
        Assert.False(actualOrderAscending);
        Assert.Equal(expectedColumnName, actualOrderColumn);
    }

    [Theory(DisplayName = "Header Click, CSS Selected Class Test")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void TableHeader_OnClick_SelectedClass_RendersCorrectly(int selectedIndex)
    {
        //Arrange
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true));

        var headerRowItems = cut.FindAll(".header-cell", true);
        var selectedItem = headerRowItems.ElementAt(selectedIndex);

        //Act
        selectedItem.Click();
        selectedItem = headerRowItems.ElementAt(selectedIndex);
        var containsSelectedClass = selectedItem.ClassList.Contains("selected");


        //Assert
        Assert.True(containsSelectedClass);
    }

    [Theory(DisplayName = "Header Click, CSS Ascending Class Test")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void TableHeader_OnClick_AscendingClass_RendersCorrectly(int selectedIndex)
    {
        //Arrange
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true));

        var headerRowItems = cut.FindAll(".header-cell", true);
        var selectedItem = headerRowItems.ElementAt(selectedIndex);

        //Act
        selectedItem.Click();
        selectedItem = headerRowItems.ElementAt(selectedIndex);
        var containsAscendingClass = selectedItem.ClassList.Contains("ascending");


        //Assert
        Assert.True(containsAscendingClass);
    }

    [Theory(DisplayName = "Header Click, CSS Descending Class Test")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void TableHeader_OnClick_DescendingClass_RendersCorrectly(int selectedIndex)
    {
        //Arrange
        var cut = RenderComponent<TableHeader<TableTestObject>>(parameters => parameters
            .Add(p => p.Headings, Headings)
            .Add(p => p.OrderColumn, string.Empty)
            .Add(p => p.OrderAscending, true));


        //Act
        cut.FindAll(".header-cell").ElementAt(selectedIndex).Click();
        cut.FindAll(".header-cell").ElementAt(selectedIndex).Click();
        var selectedItem = cut.FindAll(".header-cell").ElementAt(selectedIndex);
        var containsDescendingClass = selectedItem.ClassList.Contains("descending");

        //Assert
        Assert.True(containsDescendingClass);
    }
}
