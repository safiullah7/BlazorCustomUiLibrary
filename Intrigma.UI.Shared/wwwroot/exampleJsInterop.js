// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}

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