(function (jang, $) {
	var templatesDownloaded = false;
	var templatesDownloading = false;
	var pendingCallbacks = [];

	function __render(template, model) {
		if (model.Model == undefined) {
			model = { Model: model }; //i don't like this hack
		}

		{viewEngineRender}
	}

	function __ensureTemplatesExist(callback) {
		if (!templatesDownloaded && !templatesDownloading) {
			templatesDownloading = true;

			$.ajax({
				url: '{jangViewsUrl}',
				success: function (result) {
					$('body').append(result);
					templatesDownloaded = true;
					__ensureTemplatesExist(callback);
					for (var i in pendingCallbacks) {
						pendingCallbacks[i]();
					}
				},
				error: function (err) {
					console.log(err);
				}
			});
		} else if (templatesDownloading) {
			pendingCallbacks.push(callback);
		} else {
			callback();
		}
	}

	function __scrub(text) {
		var scrubber = /[^A-Za-z0-9_\\-\\s]/g ;
		var spaceRemover = /\\s+/g ;
		text = text.replace(scrubber, "-").trim();
		text = text.replace(spaceRemover, "-");
		text = text.replace( /^-*/ , "");
		text = text.replace( /-*$/ , "");
		text = text.replace( /--+/ , "-");
		text = text.toLowerCase();
		return text;
	}

	function __findView(view) {
		if (view[0] == '~') { //absolute path
			view = __scrub(view);
			return view;
		}

		view = __scrub(view);
		for (var i in jang.views) {
			if (jang.views[i].view == view) {
				return jang.views[i].fullPath;
			}
		}

		return null;
	}

	jang.views = [];

	jang.render = function(id, destination, model) {
		//queue this up for rendering
		__ensureTemplatesExist(function() {
			var result = $(__render(id, model));
			$('#' + destination).after(result);
		});
	};

	jang.ajax = function(settings) {
		var ajaxSettings = settings || { };
		var finalSuccess = settings.success || function(e) {
			console.log("no finalize method created. skipping render");
		};

		ajaxSettings.success = function(result) {
			finalSuccess(__render(result.template, result.model));
		};

		$.ajax(ajaxSettings);
	};

	jang.renderView = function (view, model) {
		var viewName = __findView(view);
		return __render(viewName, model);
	};
	
})(window.jang = window.jang || {}, jQuery);