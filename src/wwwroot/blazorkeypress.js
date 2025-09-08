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
        // Account for fixed header height (approximately 64px + some padding)
        const headerHeight = 120;
        const elementPosition = element.offsetTop - headerHeight;
        
        window.scrollTo({
            top: elementPosition,
            behavior: 'smooth'
        });
    }
};

window.focusFirstAnswerOption = function () {
    const firstAnswerOption = document.getElementById('answer-option-0');
    if (firstAnswerOption) {
        firstAnswerOption.focus();
        // Add a subtle highlight effect
        firstAnswerOption.style.transform = 'scale(1.02)';
        firstAnswerOption.style.boxShadow = '0 0 20px rgba(59, 130, 246, 0.5)';
        
        // Remove the highlight after a short delay
        setTimeout(() => {
            firstAnswerOption.style.transform = '';
            firstAnswerOption.style.boxShadow = '';
        }, 2000);
    }
};