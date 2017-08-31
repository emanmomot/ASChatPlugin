console.log("Background script");

// index of AS tab
var tabIndex;
// 0 disconnected 1 connecting 2 connected
var connectionStatus = 0;
// connection error message
var statusMessage = "";

chrome.extension.onMessage.addListener( function (request, sender, sendResponse) {

    if (request.type === "Open" ) {

        connectionStatus = 1;

        // get tab index every time we open connection
        chrome.tabs.query({url: "http://www.adultswim.com/videos/streams" }, function(tabs) {
            if(tabs.length > 0) {
                tabIndex = tabs[0].id;
                chrome.tabs.sendMessage(tabIndex, { type: "OpenMe", url: request.url }, function (response) {
                    if(response.success) {
                        connectionStatus = 2;
                    } else {
                        connectionStatus = 0;
                        statusMessage = response.message;
                    }

                    sendResponse(response);
                });
            } else {
                console.log("Tab not open");
                tabIndex = 0;
                connectionStatus = 0;
                statusMessage = "Tab not open.";
                sendResponse({ success: false, message: statusMessage });
            }
        });
        // async callback
        return true;

    } else if(request.type === "State") {
        sendResponse({ connectionStatus: connectionStatus, statusMessage: statusMessage });

    } else if(request.type === "CloseReq") {
        // if connection is not already closed
        if (connectionStatus != 0) {
            if(tabIndex) {
                chrome.tabs.sendMessage(tabIndex, { type: "CloseReq" });
                sendResponse({ success: true });
            } else {
                sendResponse({ success: false, message: "Tab not open." });
            }

            connectionStatus = 0;
            statusMessage = "";
        } else {
            sendResponse({ success: true });
        }

    } else if(request.type === "OnClose") {
        connectionStatus = 0;
        statusMessage = request.message;
    }
});