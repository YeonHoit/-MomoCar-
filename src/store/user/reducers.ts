import * as T from './types'

const initialState: T.State = {
  Index_ID: -1,
  ID: '',
  Login: '',
  Nickname: '',
  UserCar: '',
  Photo: '',
  Email: '',
  Phone: '',
  NoticeAlert: false,
  CommentAlert: false,
  LikeAlert: false,
  TagAlert: false,
  ChattingAlert: false,
  UserCarType: ''
}

export const reducer = (state: T.State = initialState, action: T.Actions) => {
  switch (action.type) {
    case '@user/setUser':
      return {
        Index_ID: action.Index_ID,
        ID: action.ID,
        Login: action.Login,
        Nickname: action.Nickname,
        UserCar: action.UserCar,
        Photo: action.Photo,
        Email: action.Email,
        Phone: action.Phone,
        NoticeAlert: action.NoticeAlert,
        CommentAlert: action.CommentAlert,
        LikeAlert: action.LikeAlert,
        TagAlert: action.TagAlert,
        ChattingAlert: action.ChattingAlert,
        UserCarType: action.UserCarType
      }
    case '@user/distroyUser':
      return initialState
  }
  return state
}
