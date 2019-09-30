export default class Auth {
    cookieName = "loggedIn"

    createLoggedInCookie (){
        document.cookie = this.cookieName + "=true;";
    }

    deleteLoggedInCookie (){
        document.cookie = this.cookieName + "=true; expires=expires=Thu, 01 Jan 1970 00:00:01 GMT; " ;
    }

    getLoggedInCookie (){
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for(var i = 0; i <ca.length; i++) {
          var c = ca[i];
          while (c.charAt(0) === ' ') {
            c = c.substring(1);
          }
          if (c.indexOf(this.cookieName) === 0) {
            return c.substring(this.cookieName.length, c.length);
          }
        }
        return "";
    }

    isLoggedIn(){
        var loggedIn = this.getLoggedInCookie();
        if(loggedIn){
            return true
        }
        return false
    }

    login = async function(username, password){
      const response = await fetch('/api/user/login', {
         method:'POST',
         headers:{'content-type':'application/json'},
         body:JSON.stringify({
          username: username,
          password: password
        })
      })
      const result = await response;
      var responseBody = await result.json();
      if(result.status === 400){
          return { error : responseBody.error}
        }
        else{
          this.createLoggedInCookie()
          return responseBody;
        }
    }

    logout = async function(){
      this.deleteLoggedInCookie();
      const response = await fetch('/api/user/logout', {
         method:'POST',
         headers:{'content-type':'application/json'},
      });
      await response;
      window.location.href = "/login";
    }

}
