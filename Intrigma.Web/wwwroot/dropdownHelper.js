window.dropdownHelper = {
    registerClickOutside: function (dropdownElement, dotNetHelper) {
        function outsideClickListener(event) {
            if (!dropdownElement.contains(event.target)) {
                dotNetHelper.invokeMethodAsync("CloseDropdown");
                document.removeEventListener("click", outsideClickListener);
            }
        }
        document.addEventListener("click", outsideClickListener);
    }
};
