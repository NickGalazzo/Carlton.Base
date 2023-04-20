﻿namespace Carlton.Base.Components.TestBed;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.AddCarltonTestBed(builder =>
        {
            //Base Components
            builder.AddComponent<Card>(CardTestStates.DefaultState)
                   .AddComponent<ListCard<string>>(CardTestStates.DefaultListState)
                   .AddComponent<CountCard>("Accent 1", CardTestStates.CountCard1State)
                   .AddComponent<CountCard>("Accent 2", CardTestStates.CountCard2State)
                   .AddComponent<CountCard>("Accent 3", CardTestStates.CountCard3State)
                   .AddComponent<CountCard>("Accent 4", CardTestStates.CountCard4State)
                   .AddComponent<Logo>(LogoTestStates.DefaultState)
                   .AddComponent<ProfileAvatar>(ProfileAvatarTestStates.DefaultState)
                   .AddComponent<TitleBreadCrumbs>(TitleBreadCrumbsTestStates.DefaultState)
                   .AddComponent<Checkbox>("Checked", CheckboxTestStates.CheckedState)
                   .AddComponent<Checkbox>("Unchecked", CheckboxTestStates.UncheckedState)
                   .AddComponent<Select>(SelectTestStates.Default)
                   .AddComponent<Table<TableTestStates.TableTestObject>>("Large Item List", TableTestStates.LargeItemList)
                   .AddComponent<Table<TableTestStates.TableTestObject>>("Small Item List", TableTestStates.SmallItemList)
                   .AddComponent<Table<TableTestStates.TableTestObject>>("Without Pagination Row", TableTestStates.WithOutPaginationRow)
                   .AddComponent<NotificationBar>("FadeOut Disabled", NotificationStates.NotificationBarFadeOutDisabledStated)
                   .AddComponent<NotificationBar>("FadeOut Enabled", NotificationStates.NotificationBarFadeOutEnabledStated)
                   .AddComponent<SuccessNotification>("FadeOut Disabled", NotificationStates.SuccessFadeOutDisabledState)
                   .AddComponent<SuccessNotification>("FadeOut Enabled", NotificationStates.SuccessFadeOutEnabledState)
                   .AddComponent<InfoNotification>(NotificationStates.InfoState)
                   .AddComponent<WarningNotification>(NotificationStates.WarningState)
                   .AddComponent<ErrorNotification>(NotificationStates.ErrorState)
                   .Build();
        },
        typeof(TestBedLayout).Assembly);


        builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });


        builder.RootComponents.Add<App>("app");

        await builder.Build().RunAsync().ConfigureAwait(true);
    }
}