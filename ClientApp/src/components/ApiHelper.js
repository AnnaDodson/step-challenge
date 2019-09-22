import Auth from "./Auth";

class ApiHelper {

    GraphQlApiHelper = async function(query){
      const response = await fetch('/graphql', {
         method:'POST',
         headers:{'content-type':'application/json'},
         body: query 
      })
      const result = await response;
      switch (result.status){
        case 302:
            // reroute to login
            break;
        case 401:
            var auth = new Auth()
            auth.logout()
            break;
        default:
            return result.json()
        }
    }

}

export default ApiHelper;