console.log("hello this is my script!! :)");

var ws;

var timeOutInMiliseconds = 1000;
var timeoutCallback;
// select the target node
var target = document.getElementsByClassName('MessagesContainer__messages_1FphL')[0];
 
// create an observer instance
var observer = new MutationObserver(function(mutations) {
	for (var i = mutations.length - 1; i >= 0; i--) {
		var mutation = mutations[i];
		var rootMsg = mutation.target.childNodes[mutation.target.childNodes.length-1-i];
		var timestamp = rootMsg.firstChild.firstChild.innerHTML;
		var username = rootMsg.lastChild.firstChild.innerHTML;

		//var message = rootMsg.lastChild.lastChild.innerText;
		var msgBodyNode = rootMsg.lastChild.lastChild;
		var message = '';
		for(var j = 0; j < msgBodyNode.childNodes.length; j++) {
			var node = msgBodyNode.childNodes[j];
			if(node.nodeName == '#comment') {
				continue;
			} else if(node.nodeName == '#text') {
				message += node.wholeText;
			} else if(node.nodeName == 'SPAN' && node.childNodes[0].nodeName == 'IMG') {
				message += node.childNodes[0].alt;
			} else if(node.nodeName == 'A') {
				message += node.innerText;
			}
		}

		console.log(message);
		
		
		// ws exists and is OPEN
		if (ws != null && ws.readyState == 1) {
			console.log(username + ": " + message);
			ws.send(JSON.stringify({ username: username, text: message }));
		} else if(ws != null && ws.readyState != 1) {
			console.log(username + ": " + message + " ws NOT READY");
		}
	}    
});
 
// configuration of the observer:
var config = { attributes: true, childList: true, characterData: true };
 
// pass in the target node, as well as the observer options
observer.observe(target, config);

window.addEventListener("beforeunload", function (e) {
	// close event
	if(ws && ws.readyState == 1) {
		onClose({ reason: "AS Tab closed" });
	}
});

chrome.runtime.onMessage.addListener (
  function(request, sender, sendResponse) {
    if (request.type == "OpenMe") {
    	console.log("Creating WebSocket.");
    	ws = new WebSocket(request.url);
		ws.onopen = function() {
			sendResponse({ success: true });
			window.clearTimeout(timeoutCallback);
		};
		ws.onclose = onClose;
		ws.onmessage = onMessage;
		ws.onerror = function() {
			sendResponse({ success: false, message: "Connection error. Is your unity project running?" });
		};

		if(timeoutCallback) {
			window.clearTimeout(timeoutCallback);
		}
		timeoutCallback = window.setTimeout(function() {
			if (ws && ws.readyState != 1) {
	        	ws.close();
	        	onClose( { reason: "Time Out:: Is your unity project running?" } );
	        	sendResponse({ success: false, message: "Timeout!!!" });
        	}
    	}, timeOutInMiliseconds)

		// async callback
		return true;
    } else if (request.type == "CloseReq") {
    	console.log("Closing WebSocket.");
    	if (ws) {
    		ws.close();
    		onClose({ reason: "Connection closed by user." });
    	}
    }
});

function onClose(event) {
	console.log("WS closed callback");
	if (event.reason) {
		chrome.extension.sendMessage({ type: "OnClose", message: event.reason });
	} else {
		chrome.extension.sendMessage({ type: "OnClose", message: "Connection failed. Is your unity project running?" });
	}

	ws = null;
}

function onMessage() {

}