

class AuthController {
    constructor(pathToOperation, inputFields) {
        this.pathToOperation = pathToOperation;
        this.inputFields = inputFields;
    }

    async registerUser() {
        let username = document.getElementById(this.inputFields[0]).value
        let password = document.getElementById(this.inputFields[1]).value

        let loadedPath = this.pathToOperation + "?username=" + username;
        const response = await fetch(loadedPath);
    }

}

function registerOnController() {
    authController.registerUser();
}
