import React, {useCallback, useEffect, useState, useMemo} from 'react'
import {
  StyleSheet,
  ScrollView,
  View,
  Text,
  Dimensions,
  Pressable
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  TouchableView,
  CommentView,
  AlertComponent,
  renderSuggestions,
  TouchableImage,
  OtherThing,
  ReviewComponent,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {CommunityPost, Comment, Chat, ChatRoom, Review} from '../type'
import moment from 'moment-with-locales-es6'
import AutoHeightImage from 'react-native-auto-height-image'
import {post, serverUrl} from '../server'
import {AutoFocusProvider, useAutoFocus} from '../contexts'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import Color from 'color'
import {MentionInput, Suggestion} from 'react-native-controlled-mentions'
import type {ParamList} from '../navigator/MainNavigator'
import {momentCal, priceComma} from '../utils'
import FastImage from 'react-native-fast-image'

moment.locale('ko')

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  contentsView: {
    width: '100%',
    backgroundColor: 'white',
    paddingHorizontal: 20,
    paddingVertical: 10
  },
  topView: {
    width: '100%',
    height: 60,
    flexDirection: 'row'
  },
  profileImage: {
    width: 60,
    height: 60,
    borderRadius: 30
  },
  nicknameText: {
    fontSize: 15,
    padding: 5,
    color: 'black'
  },
  dateText: {
    fontSize: 12,
    paddingLeft: 5,
    color: '#767676'
  },
  viewsText: {
    fontSize: 10,
    paddingTop: 5,
    color: '#767676'
  },
  titleText: {
    width: '100%',
    marginTop: 15,
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    paddingVertical: 5
  },
  contentsText: {
    marginTop: 10,
    fontSize: 14,
    lineHeight: 20,
    color: '#191919'
  },
  priceView: {
    marginTop: 10,
    paddingVertical: 10,
    flexDirection: 'row',
    alignItems: 'center'
  },
  wonText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'black'
  },
  priceTextInput: {
    fontSize: 18,
    marginLeft: 10,
    color: '#767676'
  },
  heart_comment_text: {
    fontSize: 13,
    paddingLeft: 5,
    color: '#767676'
  },
  likeButton: {
    width: Dimensions.get('window').width - 80,
    height: 60,
    backgroundColor: 'white',
    marginVertical: 15,
    alignItems: 'center',
    justifyContent: 'center',
    flexDirection: 'row',
    marginLeft: 40,
    borderRadius: 20
  },
  likeButtonText: {
    fontSize: 18,
    marginLeft: 5,
    color: 'black'
  },
  sendCommentView: {
    flexDirection: 'row',
    width: '100%',
    paddingHorizontal: 20,
    backgroundColor: 'white',
    paddingVertical: 10,
    borderTopWidth: 1,
    borderTopColor: '#DBDBDB',
    alignItems: 'center'
  },
  commentTextInputContainer: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#E6135A',
    borderRadius: 10,
    fontSize: 16,
    paddingBottom: 15,
    paddingTop: 10,
    paddingHorizontal: 10,
    marginRight: 15,
    width: Dimensions.get('window').width - 85
  },
  commentTextInput: {
    fontSize: 16,
    paddingVertical: -10,
    color: 'black'
  },
  absoluteView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    backgroundColor: Color('#000000').alpha(0.45).string()
  },
  selectBar: {
    width: '100%',
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20
  },
  selectItem: {
    width: Dimensions.get('window').width - 40,
    height: 70,
    marginLeft: 20,
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    alignItems: 'center'
  },
  selectItemBottom: {
    width: Dimensions.get('window').width - 40,
    height: 70,
    marginLeft: 20,
    flexDirection: 'row',
    alignItems: 'center'
  },
  selectItemText: {
    marginLeft: 10,
    fontSize: 14
  },
  otherThing_Review_View: {
    backgroundColor: 'white',
    width: '100%',
    padding: 20
  }
})

const temp: CommunityPost = {
  Index_num: 0,
  BoardName: '',
  NickName: '',
  Title: '',
  Contents: '',
  WriteDate: '',
  Views: 0,
  Photo: '',
  Price: ''
}

export default function PostDetailView() {
  //prettier-ignore
  const [communityPostInfo, setCommunityPostInfo] = useState<CommunityPost>(temp)
  const [commentList, setCommentList] = useState<Comment[]>([])
  const [comment, setComment] = useState<string>('')
  const [height, setHeight] = useState<number>(18)
  const [like, setLike] = useState<boolean>(false)
  const [likeCount, setLikeCount] = useState<number>(0)
  const [commentCount, setCommentCount] = useState<number>(0)
  const [showSelectBar, setShowSelectBar] = useState<boolean>(false)
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [errorMessage, setErrorMessage] = useState<string>('')
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false)
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false)
  //prettier-ignore
  const [showCommentSelectBar, setShowCommentSelectBar] = useState<boolean>(false)
  const [showProfileBar, setShowProfileBar] = useState<boolean>(false)
  const [isWriter, setIsWriter] = useState<boolean>(false)
  const [alertTarget, setAlertTarget] = useState<string>('')
  const [commentTarget, setCommentTarget] = useState<number>(0)
  const [views, setViews] = useState<number>(0)
  const [tagUsers, setTagUsers] = useState<Suggestion[]>([])
  const [userPhoto, setUserPhoto] = useState<string>('')
  const [reportTarget, setReportTarget] = useState<string>('')
  const [reportTargetID, setReportTargetID] = useState<string>()
  const [blockTarget, setBlockTarget] = useState<string>('')
  const [showBlockAlert, setShowBlockAlert] = useState<boolean>(false)
  const [otherThingList, setOtherThingList] = useState<CommunityPost[]>([])
  const [reviewList, setReviewList] = useState<Review[]>([])
  //prettier-ignore
  const [communityPostLoading, setCommunityPostLoading] = useState<boolean>(true)
  const [commentLoading, setCommentLoading] = useState<boolean>(true)
  const [likeSearchLoading, setLikeSearchLoading] = useState<boolean>(true)
  const [likeCountLoading, setLikeCountLoading] = useState<boolean>(true)
  const [commentCountLoading, setCommentCountLoading] = useState<boolean>(true)
  const [viewsLoading, setViewsLoading] = useState<boolean>(true)
  const [tagListLoading, setTagListLoading] = useState<boolean>(true)
  const [userPhotoLoading, setUserPhotoLoading] = useState<boolean>(true)
  const [otherLoading, setOtherLoading] = useState<boolean>(true)
  const [reviewLoading, setReviewLoading] = useState<boolean>(true)

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'PostDetailView'>>()
  const communityNumberTrash = route.params.communityNumber
  const communityNumber = communityNumberTrash.split('|')[0]
  const focus = useAutoFocus()
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const loading = useMemo(() => {
    if (
      !communityPostLoading &&
      !commentLoading &&
      !likeSearchLoading &&
      !likeCountLoading &&
      !commentCountLoading &&
      !viewsLoading &&
      !tagListLoading &&
      !userPhotoLoading &&
      !otherLoading &&
      !reviewLoading
    ) {
      return false
    }
    return true
  }, [
    communityPostLoading,
    commentLoading,
    likeSearchLoading,
    likeCountLoading,
    commentCountLoading,
    viewsLoading,
    tagListLoading,
    userPhotoLoading,
    otherLoading,
    reviewLoading
  ])

  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  const tagTarget = useMemo(() => {
    let arr = []
    let i = 0
    while (i < comment.length) {
      let frontPos = comment.indexOf('@[', i)
      if (frontPos == -1) {
        break
      }
      let middlePos = comment.indexOf('](', frontPos)
      let backPos = comment.indexOf(')', middlePos)
      arr.push(comment.substring(middlePos + 2, backPos))
      i = backPos
    }
    return arr
  }, [comment])

  const commentResult = useMemo(() => {
    return comment.replace(/@\[([^\]]+)\]\((\d*)\)/g, '@$1')
  }, [comment])

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const commentMenuClick = useCallback(
    (item: Comment) => {
      setIsWriter(item.NickName === user.Nickname)
      setCommentTarget(item.Index_num)
      setReportTarget('Comment')
      setReportTargetID(item.Index_num.toString())
      setShowCommentSelectBar(true)
    },
    [user]
  )

  const commentListView = useMemo(() => {
    return commentList.map((item, index) => (
      <CommentView
        commentInfo={item}
        key={index.toString()}
        iconPress={() => commentMenuClick(item)}
        tagPress={() => setComment(comment + '@' + item.NickName)}
        profilePress={() => {
          if (item.NickName !== user.Nickname) {
            setBlockTarget(item.NickName)
            setShowProfileBar(true)
          }
        }}
      />
    ))
  }, [commentList, comment, user])

  const insertComment = useCallback(() => {
    if (comment.length > 0) {
      let data = new FormData()
      data.append('CommunityNumber', communityNumber)
      data.append('Comments', commentResult)
      data.append('NickName', user.Nickname)
      post('InsertComment.php', data)
        .then(() => {
          let check: boolean = false
          if (tagTarget.length > 0) {
            for (let i = 0; i < tagTarget.length; i++) {
              if (communityPostInfo.NickName === tagTarget[i]) {
                check = true
              }
              let data = new FormData()
              data.append('Title', '새로운 태그')
              data.append(
                'Contents',
                user.Nickname + '님이 게시물에 태그하였습니다.'
              )
              data.append('AlertType', 'Tag')
              data.append('FromUser', user.Nickname)
              data.append('ToUser', tagTarget[i])
              data.append('Target_num', communityNumber)
              post('InsertAlert.php', data).catch((e) => {
                setErrorMessage('알림 전송에 실패하였습니다.')
                setShowErrorAlert(true)
              })
            }
          }
          if (!check && user.Nickname !== communityPostInfo.NickName) {
            let data = new FormData()
            data.append('Title', '새로운 댓글')
            data.append(
              'Contents',
              user.Nickname + '님이 회원님의 게시물에 댓글을 남겼습니다.'
            )
            data.append('AlertType', 'Comment')
            data.append('FromUser', user.Nickname)
            data.append('ToUser', communityPostInfo.NickName)
            data.append('Target_num', communityNumber)
            post('InsertAlert.php', data).catch((e) => {
              setErrorMessage('알림 전송에 실패하였습니다.')
              setShowErrorAlert(true)
            })
          }
        })
        .catch((e) => {
          setErrorMessage('댓글 전송에 실패하였습니다.')
          setShowErrorAlert(true)
        })

      setComment('')

      let data0 = new FormData()
      data0.append('CommunityNumber', communityNumber)
      data0.append('UserID', user.ID)
      post('1.0.1/GetCommentList.php', data0)
        .then((CommentInfo) => CommentInfo.json())
        .then((CommentInfoObject) => {
          const {GetCommentList} = CommentInfoObject
          setCommentList(GetCommentList)
        })
        .catch((e) => {
          setErrorMessage('댓글 목록을 불러오지 못했습니다.')
          setShowErrorAlert(true)
        })
    } else {
      setErrorMessage('댓글을 입력해주세요.')
      setShowErrorAlert(true)
    }
  }, [communityNumber, commentResult, user, tagTarget, communityPostInfo])

  const likeButtonPress = useCallback(() => {
    if (!like) {
      setLikeCount(+likeCount + 1)
      let data = new FormData()
      data.append('ID', user.ID)
      data.append('CommunityNumber', communityNumber)
      post('InsertLike.php', data)
        .then(() => {
          if (user.Nickname !== communityPostInfo.NickName) {
            let data = new FormData()
            data.append('Title', '새로운 좋아요')
            data.append(
              'Contents',
              '회원님의 게시물에 새로운 좋아요가 있습니다.'
            )
            data.append('AlertType', 'Like')
            data.append('FromUser', user.Nickname)
            data.append('ToUser', communityPostInfo.NickName)
            data.append('Target_num', communityNumber)
            post('InsertAlert.php', data).catch((e) => {
              setErrorMessage('알림 전송에 실패하였습니다.')
              setShowErrorAlert(true)
            })
          }
        })
        .catch((e) => {
          setErrorMessage('좋아요에 실패하였습니다.')
          setShowErrorAlert(true)
        })
    } else {
      setLikeCount(likeCount - 1)
      let data = new FormData()
      data.append('ID', user.ID)
      data.append('CommunityNumber', communityNumber)
      post('DeleteLike.php', data).catch((e) => {
        setErrorMessage('좋아요 취소에 실패하였습니다.')
        setShowErrorAlert(true)
      })
    }

    setLike(!like)
  }, [user, communityNumber, like, communityPostInfo, likeCount])

  const deletePostPress = useCallback(() => {
    setAlertTarget('Post')
    setShowSelectBar(false)
    setAlertMessage('정말 글을 삭제하시겠습니까?')
    setShowAlert(true)
  }, [])

  const deletePost = useCallback(() => {
    setShowAlert(false)
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    post('DeleteCommunityPost.php', data)
      .then(() => {
        navigation.canGoBack() && navigation.goBack()
      })
      .catch((e) => {
        setErrorMessage('글 삭제에 실패하였습니다.')
        setShowErrorAlert(true)
      })
  }, [communityNumber])

  const deleteCommentPress = useCallback(() => {
    setAlertTarget('Comment')
    setShowCommentSelectBar(false)
    setAlertMessage('정말 댓글을 삭제하시겠습니까?')
    setShowAlert(true)
  }, [])

  const deleteComment = useCallback(() => {
    setShowAlert(false)
    let data = new FormData()
    data.append('Index_num', commentTarget)
    post('DeleteCommunityComment.php', data).catch((e) => {
      setErrorMessage('댓글 삭제에 실패하였습니다.')
      setShowErrorAlert(true)
    })

    let data0 = new FormData()
    data0.append('CommunityNumber', communityNumber)
    data0.append('UserID', user.ID)
    post('1.0.1/GetCommentList.php', data0)
      .then((CommentInfo) => CommentInfo.json())
      .then((CommentInfoObject) => {
        const {GetCommentList} = CommentInfoObject
        setCommentList(GetCommentList)
      })
      .catch((e) => {
        setErrorMessage('댓글 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
      })
  }, [commentTarget, communityNumber, user])

  const seperateFunc = useCallback(() => {
    if (alertTarget === 'Post') {
      deletePost()
    } else if (alertTarget === 'Comment') {
      deleteComment()
    }
  }, [alertTarget])

  const renderMentionSuggestions = useMemo(() => {
    return renderSuggestions(tagUsers)
  }, [tagUsers])

  const openChatting = useCallback(() => {
    let data = new FormData()
    data.append('UserID', user.ID)
    data.append('OpponentNickName', communityPostInfo.NickName)
    post('GetLastChat.php', data)
      .then((lastChatInfo) => lastChatInfo.json())
      .then((lastChatInfoObject) => {
        const {GetLastChat} = lastChatInfoObject
        let data = new FormData()
        data.append('Nickname', communityPostInfo.NickName)
        post('GetUserID.php', data)
          .then((UserIDInfo) => UserIDInfo.json())
          .then((UserIDInfoObject) => {
            const {GetUserID} = UserIDInfoObject
            const {UserID} = GetUserID[0]
            if (GetLastChat.length > 0) {
              let tempChat: Chat = GetLastChat[0]
              let tempChatRoom: ChatRoom = {
                Opponent: UserID,
                LastChat: tempChat
              }
              navigation.navigate('ChatRoomScreen', {
                chatRoomInfo: tempChatRoom
              })
            } else {
              let tempChat: Chat = {
                Index_num: -1,
                SendUserID: user.ID,
                SendUserNickName: user.Nickname,
                ReceiveUserID: UserID,
                ReceiveUserNickName: communityPostInfo.NickName,
                SendDate: '',
                ReadCheck: 0,
                Message: '',
                Photo: ''
              }
              let tempChatRoom: ChatRoom = {
                Opponent: UserID,
                LastChat: tempChat
              }
              navigation.navigate('ChatRoomScreen', {
                chatRoomInfo: tempChatRoom
              })
            }
          })
          .catch((e) => {
            setErrorMessage('유저 정보를 불러오지 못했습니다.')
            setShowErrorAlert(true)
          })
      })
      .catch((e) => {
        setErrorMessage('마지막 채팅정보를 불러오지 못했습니다.')
        setShowErrorAlert(true)
      })
  }, [user, communityPostInfo])

  const photoMap = useMemo(() => {
    if (communityPostInfo.Photo.length != 0) {
      let arr = communityPostInfo.Photo.split('|')
      return arr.map((item, index) => (
        <AutoHeightImage
          width={Dimensions.get('window').width - 40}
          source={{uri: serverUrl + 'img/' + item}}
          style={[{marginTop: 10}]}
          key={index.toString()}
        />
      ))
    }
    return null
  }, [communityPostInfo])

  const reportPress = useCallback(() => {
    setShowSelectBar(false)
    setShowCommentSelectBar(false)
    setShowReportAlert(true)
  }, [])

  const reportAlertPress = useCallback(() => {
    let data = new FormData()
    data.append('Target', reportTarget)
    data.append('TargetID', reportTargetID)
    post('InsertReport.php', data)
    setShowReportAlert(false)
    setErrorMessage('신고가 접수 되었습니다.')
    setShowErrorAlert(true)
  }, [reportTarget, reportTargetID])

  const blockUser = useCallback(() => {
    setShowBlockAlert(false)
    let data = new FormData()
    data.append('Nickname', blockTarget)
    post('GetUserID.php', data)
      .then((UserIDInfo) => UserIDInfo.json())
      .then((UserIDInfoObject) => {
        const {GetUserID} = UserIDInfoObject
        const {UserID} = GetUserID[0]
        let data0 = new FormData()
        data0.append('UserID', user.ID)
        data0.append('Target', UserID)
        post('InsertBlock.php', data0)
          .then(() => {
            setErrorMessage('차단이 완료되었습니다.')
            setShowErrorAlert(true)
          })
          .catch((e) => {
            setErrorMessage('사용자 차단에 실패하였습니다.')
            setShowErrorAlert(true)
          })
      })
      .catch((e) => {
        setErrorMessage('해당 사용자의 정보를 불러오지 못했습니다.')
        setShowErrorAlert(true)
      })
  }, [blockTarget, user])

  const OtherThingComponent = useMemo(() => {
    return otherThingList.map((item, index) => (
      <OtherThing
        communityPostInfo={item}
        key={index}
        onPress={() => {
          navigation.navigate('PostDetailView', {
            communityNumber:
              item.Index_num.toString() + '|' + new Date().toString()
          })
        }}
      />
    ))
  }, [otherThingList])

  const ReviewComponentList = useMemo(() => {
    return reviewList
      .slice(0, 3)
      .map((item, index) => (
        <ReviewComponent reviewInfo={item} key={index.toString()} />
      ))
  }, [reviewList])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    post('GetCommunityPost.php', data)
      .then((postInfo) => postInfo.json())
      .then((postInfoObject) => {
        const {GetCommunityPost} = postInfoObject
        setCommunityPostInfo(GetCommunityPost[0])
      })
      .then(() => setCommunityPostLoading(false))
      .catch((e) => {
        setErrorMessage('글 정보를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setCommunityPostLoading(false)
      })
  }, [communityNumber])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    data.append('UserID', user.ID)
    post('1.0.1/GetCommentList.php', data)
      .then((CommentInfo) => CommentInfo.json())
      .then((CommentInfoObject) => {
        const {GetCommentList} = CommentInfoObject
        setCommentList(GetCommentList)
      })
      .then(() => setCommentLoading(false))
      .catch((e) => {
        setErrorMessage('댓글 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setCommentLoading(false)
      })

    let data0 = new FormData()
    data0.append('ID', user.ID)
    data0.append('CommunityNumber', communityNumber)
    post('GetLikeSearch.php', data0)
      .then((LikeInfo) => LikeInfo.json())
      .then((LikeInfoObject) => {
        const {GetLikeSearch} = LikeInfoObject
        if (GetLikeSearch[0]) {
          setLike(true)
        } else {
          setLike(false)
        }
      })
      .then(() => setLikeSearchLoading(false))
      .catch((e) => {
        setErrorMessage('좋아요 정보를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setLikeSearchLoading(false)
      })
  }, [communityNumber, user])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    post('GetLikeCount.php', data)
      .then((LikeCountInfo) => LikeCountInfo.json())
      .then((LikeCountInfoObject) => {
        const {GetLikeCount} = LikeCountInfoObject
        setLikeCount(GetLikeCount[0].Count)
      })
      .then(() => setLikeCountLoading(false))
      .catch((e) => {
        setErrorMessage('좋아요 개수를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setLikeCountLoading(false)
      })
  }, [communityNumber])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    post('GetCommentCount.php', data)
      .then((CommentCountInfo) => CommentCountInfo.json())
      .then((CommentCountInfoObject) => {
        const {GetCommentCount} = CommentCountInfoObject
        setCommentCount(GetCommentCount[0].Count)
      })
      .then(() => setCommentCountLoading(false))
      .catch((e) => {
        setErrorMessage('댓글 개수를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setCommentCountLoading(false)
      })
  }, [communityNumber, commentList])

  useEffect(() => {
    let data = new FormData()
    data.append('Index_num', communityNumber)
    post('GetCommunityPostViews.php', data)
      .then((PostViewsInfo) => PostViewsInfo.json())
      .then((PostViewsInfoObject) => {
        const {GetCommunityPostViews} = PostViewsInfoObject
        setViews(GetCommunityPostViews[0].Views)
      })
      .then(() => setViewsLoading(false))
      .catch((e) => {
        setErrorMessage('조회수를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setViewsLoading(false)
      })
  }, [communityNumber])

  useEffect(() => {
    let arr: Suggestion[] = []
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    data.append('User', user.Nickname)
    post('GetTagList.php', data)
      .then((TagListInfo) => TagListInfo.json())
      .then((TagListInfoObject) => {
        const {GetTagList} = TagListInfoObject
        for (let i = 0; i < GetTagList.length; i++) {
          arr.push({
            id: GetTagList[i].ID,
            name: GetTagList[i].Nickname
          })
        }
        setTagUsers(arr)
      })
      .then(() => setTagListLoading(false))
      .catch((e) => {
        setErrorMessage('네트워크 통신에 오류가 발생했습니다.')
        setShowErrorAlert(true)
        setTagListLoading(false)
      })
  }, [commentList, communityNumber])

  useEffect(() => {
    let data = new FormData()
    data.append('NickName', communityPostInfo.NickName)
    post('GetCommunityUserPhoto.php', data)
      .then((CommunityUserPhotoInfo) => CommunityUserPhotoInfo.json())
      .then((CommunityUserPhotoInfoObject) => {
        const {GetCommunityUserPhoto} = CommunityUserPhotoInfoObject
        setUserPhoto(GetCommunityUserPhoto[0].Photo)
      })
      .then(() => setUserPhotoLoading(false))
      .catch((e) => {
        setUserPhotoLoading(false)
      })
  }, [communityPostInfo])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    data.append('NickName', communityPostInfo.NickName)
    post('GetOtherThing.php', data)
      .then((OtherThingInfo) => OtherThingInfo.json())
      .then((OtherThingInfoObject) => {
        const {GetOtherThing} = OtherThingInfoObject
        setOtherThingList(GetOtherThing)
      })
      .then(() => setOtherLoading(false))
      .catch((e) => {
        setErrorMessage('판매 내역 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setOtherLoading(false)
      })
  }, [communityNumber, communityPostInfo])

  useEffect(() => {
    let data = new FormData()
    data.append('NickName', communityPostInfo.NickName)
    post('GetReviewList.php', data)
      .then((ReviewListInfo) => ReviewListInfo.json())
      .then((ReviewListInfoObject) => {
        const {GetReviewList} = ReviewListInfoObject
        setReviewList(GetReviewList)
      })
      .then(() => setReviewLoading(false))
      .catch((e) => {
        setErrorMessage('후기 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setReviewLoading(false)
      })
  }, [communityPostInfo])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
        <SafeAreaView style={[styles.view]}>
          <NavigationHeader
            viewStyle={[{backgroundColor: 'white'}]}
            Left={() => (
              <Icon
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
            Right={() => (
              <Icon
                source={require('../assets/images/three_dots_vertical.png')}
                size={30}
                onPress={() => {
                  setReportTarget('Community')
                  setReportTargetID(communityNumber)
                  setShowSelectBar(true)
                }}
              />
            )}
          />
          <AutoFocusProvider>
            <ScrollView style={[{flex: 1, backgroundColor: '#F8F8FA'}]}>
              <View style={[styles.contentsView]}>
                <View style={[styles.topView]}>
                  <TouchableImage
                    source={profileImage}
                    style={[styles.profileImage]}
                    onPress={() => {
                      if (user.Nickname !== communityPostInfo.NickName) {
                        setBlockTarget(communityPostInfo.NickName)
                        setShowProfileBar(true)
                      }
                    }}
                  />
                  <View style={[{flex: 1, marginLeft: 5}]}>
                    <Text style={[styles.nicknameText]}>
                      {communityPostInfo.NickName}
                    </Text>
                    <Text style={[styles.dateText]}>
                      {momentCal(communityPostInfo.WriteDate)}
                    </Text>
                  </View>
                  <Text style={[styles.viewsText]}>조회 {views}</Text>
                </View>
                <View style={[styles.titleText]}>
                  <Text style={[{fontSize: 15, fontWeight: 'bold'}]}>
                    {communityPostInfo.Title}
                  </Text>
                </View>
                {communityPostInfo.BoardName === 'UsedMarket' && (
                  <View style={[styles.priceView]}>
                    <Text style={[styles.wonText]}>￦</Text>
                    <Text style={[styles.priceTextInput]}>
                      {priceComma(communityPostInfo.Price)}
                    </Text>
                  </View>
                )}
                <Text
                  style={[styles.contentsText]}
                  numberOfLines={6}
                  ellipsizeMode="tail">
                  {communityPostInfo.Contents}
                </Text>
                {photoMap}
                <View style={[{flexDirection: 'row', marginTop: 10}]}>
                  {user.ID === 'guest' && (
                    <TouchableView
                      viewStyle={{flexDirection: 'row'}}
                      onPress={() => {
                        setErrorMessage('로그인이 필요한 서비스입니다.')
                        setShowErrorAlert(true)
                      }}>
                      <FastImage
                        source={require('../assets/images/heart_empty.png')}
                        style={{width: 30, height: 30}}
                      />
                      <View style={[{height: 30, justifyContent: 'center'}]}>
                        <Text style={[styles.heart_comment_text]}>
                          {likeCount}
                        </Text>
                      </View>
                    </TouchableView>
                  )}
                  {user.ID !== 'guest' && (
                    <TouchableView
                      viewStyle={[{flexDirection: 'row'}]}
                      onPress={likeButtonPress}>
                      {like && (
                        <FastImage
                          source={require('../assets/images/heart_fill.png')}
                          style={[{width: 30, height: 30}]}
                        />
                      )}
                      {!like && (
                        <FastImage
                          source={require('../assets/images/heart_empty.png')}
                          style={[{width: 30, height: 30}]}
                        />
                      )}
                      <View style={[{height: 30, justifyContent: 'center'}]}>
                        <Text style={[styles.heart_comment_text]}>
                          {likeCount}
                        </Text>
                      </View>
                    </TouchableView>
                  )}
                  <FastImage
                    style={[{width: 30, height: 30, marginLeft: 50}]}
                    source={require('../assets/images/comment.png')}
                  />
                  <View style={[{height: 30, justifyContent: 'center'}]}>
                    <Text style={[styles.heart_comment_text]}>
                      {commentCount}
                    </Text>
                  </View>
                </View>
              </View>
              {commentListView}
              {communityPostInfo.BoardName === 'UsedMarket' && (
                <View style={[styles.otherThing_Review_View]}>
                  <View style={{flexDirection: 'row', alignItems: 'center'}}>
                    <Text style={{fontSize: 15, fontWeight: 'bold', flex: 1}}>
                      {communityPostInfo.NickName}님의 상품
                    </Text>
                    <Text
                      style={{fontSize: 12, color: '#999999'}}
                      onPress={() =>
                        navigation.navigate('OtherThingScreen', {
                          communityNumber:
                            communityNumber + '|' + new Date().toString(),
                          NickName: communityPostInfo.NickName
                        })
                      }>
                      더 알아보기 {'>'}
                    </Text>
                  </View>
                  <ScrollView
                    contentContainerStyle={{marginTop: 10}}
                    horizontal={true}
                    showsHorizontalScrollIndicator={false}
                    overScrollMode="never">
                    {OtherThingComponent}
                  </ScrollView>
                </View>
              )}
              {communityPostInfo.BoardName === 'UsedMarket' && (
                <View style={[styles.otherThing_Review_View]}>
                  <View style={{flexDirection: 'row', alignItems: 'center'}}>
                    <Text style={{fontSize: 15, fontWeight: 'bold', flex: 1}}>
                      후기 {reviewList.length}
                    </Text>
                    <Text
                      style={{fontSize: 12, color: '#999999'}}
                      onPress={() =>
                        navigation.navigate('ReviewScreen', {
                          NickName: communityPostInfo.NickName
                        })
                      }>
                      더 알아보기 {'>'}
                    </Text>
                  </View>
                  {ReviewComponentList}
                </View>
              )}
            </ScrollView>
          </AutoFocusProvider>
          {user.ID !== 'guest' && (
            <View style={[styles.sendCommentView]}>
              <MentionInput
                style={[styles.commentTextInput]}
                onFocus={focus}
                placeholder="댓글 입력"
                placeholderTextColor="#DBDBDB"
                multiline={true}
                scrollEnabled={false}
                onContentSizeChange={(e) =>
                  setHeight(e.nativeEvent.contentSize.height)
                }
                value={comment}
                onChange={setComment}
                partTypes={[
                  {
                    trigger: '@',
                    renderSuggestions: renderMentionSuggestions
                  }
                ]}
                containerStyle={[
                  styles.commentTextInputContainer
                  // {height: Platform.select({android: height})}
                ]}
              />
              <Icon
                source={require('../assets/images/navigation.png')}
                size={30}
                onPress={insertComment}
              />
            </View>
          )}
        </SafeAreaView>
      )}
      {showSelectBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowSelectBar(false)}
          />
          {user.Nickname === communityPostInfo.NickName && (
            <View style={[styles.selectBar]}>
              <TouchableView
                viewStyle={[styles.selectItem]}
                onPress={() =>
                  navigation.navigate('ModifyPost', {communityNumber})
                }>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/modify_background.png')}
                />
                <Text style={[styles.selectItemText]}>글 수정하기</Text>
              </TouchableView>
              <TouchableView
                viewStyle={[styles.selectItemBottom]}
                onPress={deletePostPress}>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/delete_background.png')}
                />
                <Text style={[styles.selectItemText]}>삭제하기</Text>
              </TouchableView>
            </View>
          )}
          {user.Nickname !== communityPostInfo.NickName && (
            <View style={[styles.selectBar]}>
              <TouchableView
                viewStyle={[styles.selectItemBottom]}
                onPress={reportPress}>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/report_background.png')}
                />
                <Text style={[styles.selectItemText]}>신고하기</Text>
              </TouchableView>
            </View>
          )}
        </View>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={true}
          okPress={seperateFunc}
          cancelPress={() => setShowAlert(false)}
        />
      )}
      {showCommentSelectBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowCommentSelectBar(false)}
          />
          {isWriter && (
            <View style={[styles.selectBar]}>
              <TouchableView
                viewStyle={[styles.selectItem]}
                onPress={deleteCommentPress}>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/delete_background.png')}
                />
                <Text style={[styles.selectItemText]}>삭제하기</Text>
              </TouchableView>
            </View>
          )}
          {!isWriter && (
            <View style={[styles.selectBar]}>
              <TouchableView
                viewStyle={[styles.selectItem]}
                onPress={reportPress}>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/report_background.png')}
                />
                <Text style={[styles.selectItemText]}>신고하기</Text>
              </TouchableView>
            </View>
          )}
        </View>
      )}
      {showProfileBar && communityPostInfo.NickName !== user.Nickname && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowProfileBar(false)}
          />
          <View style={[styles.selectBar]}>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setShowProfileBar(false)
                setShowBlockAlert(true)
              }}>
              <FastImage
                style={{width: 35, height: 35}}
                source={require('../assets/images/ban_background.png')}
              />
              <Text style={[styles.selectItemText]}>차단하기</Text>
            </TouchableView>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setReportTarget('Community')
                setReportTargetID(communityNumber)
                setShowProfileBar(false)
                reportPress()
              }}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/report_background.png')}
              />
              <Text style={[styles.selectItemText]}>신고하기</Text>
            </TouchableView>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={openChatting}>
              <FastImage
                style={{width: 35, height: 35}}
                source={require('../assets/images/comment_background.png')}
              />
              <Text style={[styles.selectItemText]}>채팅하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {showErrorAlert && (
        <AlertComponent
          message={errorMessage}
          cancelUse={false}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
      {showReportAlert && (
        <AlertComponent
          message="신고하시겠습니까?"
          cancelUse={true}
          okPress={reportAlertPress}
          cancelPress={() => setShowReportAlert(false)}
        />
      )}
      {showBlockAlert && (
        <AlertComponent
          message="정말 차단하시겠습니까?"
          cancelUse={true}
          okPress={blockUser}
          cancelPress={() => setShowBlockAlert(false)}
        />
      )}
    </>
  )
}
