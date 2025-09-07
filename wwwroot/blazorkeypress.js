window.blazorKeyPressed = function (dotnetHelper) {
    document.addEventListener('keyup', function (event) {
        dotnetHelper.invokeMethodAsync('OnArrowKeyPressed', event.key);
    });
};

window.addClickOutsideHandler = function (dotnetHelper) {
    document.addEventListener('click', function (event) {
        // Check if click is outside the search component
        const searchComponent = document.querySelector('[data-search-component]');
        if (searchComponent && !searchComponent.contains(event.target)) {
            dotnetHelper.invokeMethodAsync('OnClickOutside');
        }
    });
};

window.scrollToElement = function (elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};