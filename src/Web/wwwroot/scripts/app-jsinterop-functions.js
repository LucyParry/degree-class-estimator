window.jsInteropFunctions = {

    sectionJump: function (sectionName) {
        if (window.innerWidth < 901) {
            window.location.hash = sectionName;
            document.getElementById(sectionName).scrollIntoView();
        }    
    },

    clipboardCopy: function (text) {
        navigator.clipboard.writeText(text).then(function () {

            let button = document.getElementById("button-copy-uri");
            let icon = document.getElementById("clipboard-copy-icon");

            button.classList.replace("btn-outline-secondary", "btn-success");
            icon.classList.replace("bi-clipboard-plus", "bi-clipboard-check");

            setTimeout(function () {
                button.classList.replace("btn-success", "btn-outline-secondary");
                icon.classList.replace("bi-clipboard-check", "bi-clipboard-plus");
            }, 2000);

        })
        .catch(function (error) {
            alert(error);
        });
    }
};