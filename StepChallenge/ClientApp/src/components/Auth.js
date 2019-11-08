export default class Auth {
    cookieNames = {
      loggedIn : "loggedIn",
      isAdmin : "isAdmin"
    }

    createLoggedInCookies (session){
        document.cookie = this.cookieNames.loggedIn + "=true;";
        document.cookie = this.cookieNames.isAdmin + "=" + session.isAdmin + ";";
    }

    setIsAdminCookie (isAdmin){
        document.cookie = this.cookieNames.isAdmin + "=" + isAdmin + ";";
    }

    deleteLoggedInCookies (){
        document.cookie = this.cookieNames.loggedIn + "=true; expires=expires=Thu, 01 Jan 1970 00:00:01 GMT; " ;
    }

    getCookie(cname) {
      var name = cname + "=";
      var decodedCookie = decodeURIComponent(document.cookie);
      var ca = decodedCookie.split(';');
      for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
          c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
          return c.substring(name.length, c.length);
        }
      }
      return "";
    }

    isLoggedIn(){
        var loggedIn = this.getCookie(this.cookieNames.loggedIn);
        if(loggedIn === 'true'){
            return true
        }
        return false
    }

    isAdmin(){
        var isAdmin = this.getCookie(this.cookieNames.isAdmin);
        if(isAdmin === 'true'){
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
          console.log(responseBody.errorLogMessage)
          return { error : responseBody.error}
        }
        else{
          this.createLoggedInCookies(responseBody)
          return responseBody;
        }
    }

    logout = async function(){
      this.deleteLoggedInCookies();
      const response = await fetch('/api/user/logout', {
         method:'POST',
         headers:{'content-type':'application/json'},
      });
      await response;
      window.location.href = "/login";
    }

    isAdminRequest = async function(){
      const response = await fetch('/api/user/is_admin', {
         method:'GET',
         headers:{'content-type':'application/json'},
      })
      var result = await response.json();
      if(result.hasOwnProperty("isAdmin")){
        this.setIsAdminCookie (result.isAdmin)
        return result.isAdmin;
      }
      else{
        return false;
      }
    }

    registerUser = async function(name, email, password, team){
        const response = await fetch('/api/register', {
         method:'POST',
         headers:{'content-type':'application/json'},
         body:JSON.stringify({
          name: name,
          email: email,
          password: password,
          teamId: team
        })
      });
      const result = await response;
      const responseBody = await response.json();
      if(result.status === 400){
          console.log(responseBody.errorLogMessage)
          return { error : responseBody.error}
        }
        else{
          this.createLoggedInCookies(responseBody)
          return responseBody;
        }
    }
}
