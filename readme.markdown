Jang
----

Jang was born out of a desire to have a single template that works on both server and client side using any view engine necessary. 
For instance an ajax site that polls for a new comment and inserts it into the page without a post back you'll have to maintain two views
one for the server-side generation of the initial page and one for the client-side rendering with an ajax call. 

Jang makes it easier to have the best of both worlds. With a single call you can render the same view on the server, while at the same time make a call
on the client and render the same view with new data.

Example
-------
Here is a sample view that generates a list of users from a model.

	//userlist.jazz
	<ul>
		@for(var i in model) {
			@jang.renderView("user", model[i])
		}
	</ul>
	
The above view makes a call to renderView()

	//user.jazz
	<li>@model.Name</li>
	
To call this from a typical razor view:

@Html.RenderTemplate("userlist", Model)

Conversely to call it from the client you can do it in one of two ways:
	
	jang.renderView("user", model)
	
This will return the html of the rendered view which you can then use to insert into the dom.

You can also make use of Ajax.

	jang.ajax({
		url: '/home/user',
		success: function (result) {
			$("ul#users").append($(result));
		}
	});

The "result" in success will contain the rendered html from the server side call.

    public ActionResult User()
    {
        return new JangResult(new User {Name = "this is a new user "});
    }

The JangResult automatically determines the view name based on the action or you can specify a different one yourself.

The goal was to make this seemless and invisible to the consumer.