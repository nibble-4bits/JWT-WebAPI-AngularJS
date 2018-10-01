/**
 * @description A method that receives a JWT as input and returns the expiration date of that token
 * @param {string} token
 */
function getExpirationDate(token) {
    var tokenParts = token.split(".");
    var payloadDecoded = JSON.parse(atob(tokenParts[1]));

    return new Date(payloadDecoded.exp * 1000);
}