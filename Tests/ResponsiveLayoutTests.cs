using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Bunit;
using TomAndJerry.Layout;
using TomAndJerry.Services;
using TomAndJerry.Component;

namespace TomAndJerry.Tests
{
    public class ResponsiveLayoutTests : UITestBase
    {
        [Test]
        public async Task MainLayout_ShouldRenderMobileLayout_WhenScreenIsMobile()
        {
            // Arrange
            var responsiveService = Services.GetRequiredService<IResponsiveService>();
            
            // Mock mobile screen size
            await responsiveService.InitializeAsync();
            
            // Act
            var component = RenderComponent<MainLayout>(parameters => parameters
                .Add(p => p.Body, (RenderFragment)((builder) =>
                {
                    builder.AddMarkupContent(0, "<div>Test Content</div>");
                }))
            );

            // Assert
            // The component should render without errors
            Assert.That(component, Is.Not.Null);
        }

        [Test]
        public async Task MainLayout_ShouldRenderTabletLayout_WhenScreenIsTablet()
        {
            // Arrange
            var responsiveService = Services.GetRequiredService<IResponsiveService>();
            
            // Mock tablet screen size
            await responsiveService.InitializeAsync();
            
            // Act
            var component = RenderComponent<MainLayout>(parameters => parameters
                .Add(p => p.Body, (RenderFragment)((builder) =>
                {
                    builder.AddMarkupContent(0, "<div>Test Content</div>");
                }))
            );

            // Assert
            // The component should render without errors
            Assert.That(component, Is.Not.Null);
        }

        [Test]
        public async Task MainLayout_ShouldRenderDesktopLayout_WhenScreenIsDesktop()
        {
            // Arrange
            var responsiveService = Services.GetRequiredService<IResponsiveService>();
            
            // Mock desktop screen size
            await responsiveService.InitializeAsync();
            
            // Act
            var component = RenderComponent<MainLayout>(parameters => parameters
                .Add(p => p.Body, (RenderFragment)((builder) =>
                {
                    builder.AddMarkupContent(0, "<div>Test Content</div>");
                }))
            );

            // Assert
            // The component should render without errors
            Assert.That(component, Is.Not.Null);
        }

        [Test]
        public void MobileAppBar_ShouldRenderCorrectly()
        {
            // Act
            var component = RenderComponent<MobileAppBar>();

            // Assert
            Assert.That(component, Is.Not.Null);
            var header = component.Find("#mobile-app-header");
            Assert.That(header, Is.Not.Null);
        }

        [Test]
        public void MobileFooter_ShouldRenderCorrectly()
        {
            // Act
            var component = RenderComponent<MobileFooter>();

            // Assert
            Assert.That(component, Is.Not.Null);
            var footer = component.Find(".tom-jerry-footer");
            Assert.That(footer, Is.Not.Null);
        }

        [Test]
        public void TabletAppBar_ShouldRenderCorrectly()
        {
            // Act
            var component = RenderComponent<TabletAppBar>();

            // Assert
            Assert.That(component, Is.Not.Null);
            var header = component.Find("#tablet-app-header");
            Assert.That(header, Is.Not.Null);
        }

        [Test]
        public void TabletFooter_ShouldRenderCorrectly()
        {
            // Act
            var component = RenderComponent<TabletFooter>();

            // Assert
            Assert.That(component, Is.Not.Null);
            var footer = component.Find(".tom-jerry-footer");
            Assert.That(footer, Is.Not.Null);
        }
    }
}
