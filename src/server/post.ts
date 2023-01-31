export const serverUrl: string = ''

export const post = (url: string, data: FormData) =>
  fetch(serverUrl + 'ReactNative/' + url, {
    method: 'POST',
    headers: {
      Accept: 'application/json'
    },
    body: data
  })

export const post_without_data = (url: string) =>
  fetch(serverUrl + 'ReactNative/' + url, {
    method: 'POST'
  })
