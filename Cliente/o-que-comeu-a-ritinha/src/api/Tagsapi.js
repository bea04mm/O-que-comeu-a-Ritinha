export function getTagsAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Tags", {
        method: "GET",
        redirect: "follow"
    });
}

export function getTagAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Tags/${id}`, {
        method: "GET",
        redirect: "follow"
    });
}

export function postTagAPI(tag) {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Tags/PostTags", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Tag: tag }),
        redirect: "follow"
    });
}

export function putTagAPI(tag) {
    const { id, tag: tagName } = tag;

    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Tags/PutTags/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            Id: id,
            Tag: tagName
        }),
        redirect: "follow"
    });
}

export function deleteTagAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Tags/DeleteTags/${id}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        redirect: "follow"
    });
}