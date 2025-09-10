window.responsiveService = {
    dotNetRef: null,
    
    initialize: function (dotNetRef) {
        this.dotNetRef = dotNetRef;
        this.checkBreakpoint();
        window.addEventListener('resize', () => this.checkBreakpoint());
    },
    
    checkBreakpoint: function () {
        const width = window.innerWidth;
        const isMobile = width < 768;
        const isTablet = width >= 768 && width < 1024;
        const isDesktop = width >= 1024;
        
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnBreakpointChange', isMobile, isTablet, isDesktop);
        }
    },
    
    isMobile: function () {
        return window.innerWidth < 768;
    },
    
    isTablet: function () {
        const width = window.innerWidth;
        return width >= 768 && width < 1024;
    },
    
    isDesktop: function () {
        return window.innerWidth >= 1024;
    }
};
