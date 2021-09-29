window.app = {
    updateScroll: () => {
        const element = document.getElementById("chatContainer") //document.querySelector('html');
        element.scrollTop = element.scrollHeight - element.clientHeight;
    },
    scroll: () => {
        const chatbox = document.getElementById("chatContainer") //document.querySelector('html');
        chatbox.focus();
        chatbox.scrollIntoView(false);
        //element.scrollTop = element.scrollHeight - element.clientHeight;
    }
};
