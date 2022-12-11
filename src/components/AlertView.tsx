import React, {useMemo} from 'react'
import type {FC, ComponentProps} from 'react'
import {StyleSheet, Pressable, Dimensions, View, Text} from 'react-native'
import type {AlertType} from '../type'
import moment from 'moment-with-locales-es6'
import {momentCal} from '../utils'
import FastImage from 'react-native-fast-image'

moment.locale('ko')

const styles = StyleSheet.create({
  view: {
    width: Dimensions.get('window').width,
    backgroundColor: 'white',
    paddingVertical: 10,
    paddingLeft: 10,
    alignItems: 'center',
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderColor: '#EEEEEE'
  },
  image: {
    width: 50,
    height: 50
  },
  titleText: {
    fontSize: 14,
    flex: 1,
    color: 'black'
  },
  dateTimeText: {
    fontSize: 10,
    color: '#767676',
    marginRight: 15
  },
  contentsText: {
    fontSize: 14,
    color: '#767676',
    marginBottom: 5
  }
})

type PressableProps = ComponentProps<typeof Pressable>

// 알림 목록 컴포넌트 매개변수 타입 정의
export type AlertViewProps = PressableProps & {
  alertInfo: AlertType // 알림 정보 매개변수
}

// 알림 목록 컴포넌트 정의
export const AlertView: FC<AlertViewProps> = ({
  alertInfo,
  ...pressableProps
}) => {
  // 알림 종류에 따른 이미지 컴포넌트를 반환하는 useMemo
  const alertImage = useMemo(() => {
    if (alertInfo.AlertType === 'Comment') {
      return (
        <FastImage
          source={require('../assets/images/alert_comment.png')}
          style={[styles.image]}
        />
      )
    } else if (alertInfo.AlertType === 'Like') {
      return (
        <FastImage
          source={require('../assets/images/alert_like.png')}
          style={[styles.image]}
        />
      )
    } else if (alertInfo.AlertType === 'Notice') {
      return (
        <FastImage
          source={require('../assets/images/alert_notice.png')}
          style={[styles.image]}
        />
      )
    }
    return (
      <FastImage
        source={require('../assets/images/alert_tag.png')}
        style={[styles.image]}
      />
    )
  }, [alertInfo])

  return (
    <Pressable style={[styles.view]} {...pressableProps}>
      {/* 알림 이미지 */}
      {alertImage}
      {/* 내용 컴포넌트 */}
      <View style={{marginLeft: 10, flex: 1}}>
        <View
          style={{
            flexDirection: 'row',
            marginBottom: 2
          }}>
          {/* 알림 제목 */}
          <Text style={[styles.titleText]}>{alertInfo.Title}</Text>
        </View>
        {/* 알림 내용 */}
        <Text style={[styles.contentsText]}>{alertInfo.Contents}</Text>
        {/* 알림 시간 */}
        <Text style={[styles.dateTimeText]}>
          {momentCal(alertInfo.AlertDate)}
        </Text>
      </View>
    </Pressable>
  )
}
