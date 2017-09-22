
new function() {
	var ws = null;
	var adultswimTab;

	var serverUrl;
	var connectionStatus;
	var sendMessage;
	
	var connectButton;
	var disconnectButton; 
	var sendButton;


	chrome.extension.onMessage.addListener( function(request,sender,sendResponse) {
	    if(request.type === "OnClose") {
	        onClose(request.message);
	    }	
	});

	var open = function() {
		var url = serverUrl.val();
		connectionStatus.text('Connecting...');
		serverUrl.attr('disabled', 'disabled');
		connectButton.hide();
		disconnectButton.show();
		chrome.extension.sendMessage({ type: "Open", url: url }, function(response) {
			if(response.success){
				onOpen();
			} else {
				connectionStatus.text('Disconnected - ' + response.message);
				serverUrl.removeAttr('disabled');
				connectButton.show();
				disconnectButton.hide();
			}
		});
	}
	
	var close = function() {
		chrome.extension.sendMessage({ type: "CloseReq" }, function(response) {
			if(!response.success) {
				connectionStatus.text("Closing failed: " + response.message);
			}
		});

		connectionStatus.text('Closing');
	}
	
	var clearLog = function() {
		$('#messages').html('');
	}
	
	var onOpen = function() {
		
		connectionStatus.text('Connected');
		sendMessage.removeAttr('disabled');
		sendButton.removeAttr('disabled');
	};
	
	var onClose = function(message) {

		connectionStatus.text('Disconnected - ' + message);

		serverUrl.removeAttr('disabled');
		connectButton.show();
		disconnectButton.hide();
		sendMessage.attr('disabled', 'disabled');
		sendButton.attr('disabled', 'disabled');
	};
	
	var onMessage = function(event) {
		var data = event.data;
		addMessage(data);
	};
	
	var onError = function(event) {
		alert(event.data);
	}
	
	var addMessage = function(data, type) {
		var msg = $('<pre>').text(data);
		if (type === 'SENT') {
			msg.addClass('sent');
		}
		var messages = $('#messages');
		messages.append(msg);
		
		var msgBox = messages.get(0);
		while (msgBox.childNodes.length > 1000) {
			msgBox.removeChild(msgBox.firstChild);
		}
		msgBox.scrollTop = msgBox.scrollHeight;
	}

	WebSocketClient = {
		init: function() {
			serverUrl = $('#serverUrl');
			connectionStatus = $('#connectionStatus');
			sendMessage = $('#sendMessage');
			
			connectButton = $('#connectButton');
			disconnectButton = $('#disconnectButton'); 
			sendButton = $('#sendButton');
			
			connectButton.click(function(e) {
				close();
				open();
			});
		
			disconnectButton.click(function(e) {
				close();
			});
			
			sendButton.click(function(e) {
				var msg = $('#sendMessage').val();
				addMessage(msg, 'SENT');
				ws.send(msg);
			});
			
			$('#clearMessage').click(function(e) {
				clearLog();
			});
			
			var isCtrl;
			sendMessage.keyup(function (e) {
				if(e.which == 17) isCtrl=false;
			}).keydown(function (e) {
				if(e.which == 17) isCtrl=true;
				if(e.which == 13 && isCtrl == true) {
					sendButton.click();
					return false;
				}
			});

			// get initial state when popup opens
			chrome.extension.sendMessage({type: "State"}, function(response) {
				if (response.connectionStatus == 2) {
					serverUrl.attr('disabled', 'disabled');
					connectButton.hide();
					disconnectButton.show();
					onOpen();
				} else if (response.connectionStatus == 1) {
					connectionStatus.text('Connecting...');
					serverUrl.attr('disabled', 'disabled');
					connectButton.hide();
					disconnectButton.show();
				} else {
					connectionStatus.text('Disconnected - ' + response.statusMessage);
					serverUrl.removeAttr('disabled');
					connectButton.show();
					disconnectButton.hide();
				}
			});
		}
	};
}

$(function() {
	WebSocketClient.init();
});


