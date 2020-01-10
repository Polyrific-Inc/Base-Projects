const WEBSITE_REGEX = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;

const validateWebsite = (website) => {
    return website ? WEBSITE_REGEX.test(website) : true;
}

const VALIDATOR_ERROR_MESSAGE = {
    website: "Invalid Website URL Format!"
}

export {
    validateWebsite,
    VALIDATOR_ERROR_MESSAGE,
}