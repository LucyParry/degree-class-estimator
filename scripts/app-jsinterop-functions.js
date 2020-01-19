window.jsInteropFunctions = {
    getLocationOrigin: function () {
        return window.location.origin;
    },

    sectionJump: function (sectionName) {
        if (window.view.width < 901) {
            window.location.hash = sectionName;
        }    
    },

    clipboardCopy: function () {
        var urlInput = document.getElementById("generated-url");

        // get current selection so we can clear it afterwards
        var selection = window.getSelection();
        // select all text in the input and copy
        urlInput.select();
        document.execCommand('copy');
        // reset selection
        selection.removeAllRanges();
    }
};