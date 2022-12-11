import React from 'react'
import type {FC, ReactNode} from 'react'
import {StyleSheet, View, Text} from 'react-native'
import type {StyleProp, ViewStyle, TextStyle} from 'react-native'

const styles = StyleSheet.create({
  view: {
    width: '100%',
    height: 50,
    padding: 10,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between'
  },
  title: {
    fontSize: 14,
    fontWeight: 'bold',
    textAlign: 'center'
  },
  flex: {
    flex: 1,
    backgroundColor: 'transparent',
    alignItems: 'center',
    justifyContent: 'center'
  },
  left_right_view: {
    minWidth: 30,
    height: 30,
    marginHorizontal: 10,
    alignItems: 'center',
    justifyContent: 'center'
  }
})

// 상단 바 컴포넌트 매개변수 정의
export type NavigationHeaderProps = {
  title?: string // 상단 바 가운데 텍스트 매개변수
  Left?: () => ReactNode // 상단 바 왼쪽 컴포넌트 매개변수
  Right?: () => ReactNode // 상단 바 오른쪽 컴포넌트 매개변수
  viewStyle?: StyleProp<ViewStyle> // 상단 바 스타일 매개변수
  titleStyle?: StyleProp<TextStyle> // 상단 바 가운데 텍스트 스타일 매개변수
}

// 상단 바 컴포넌트 정의
export const NavigationHeader: FC<NavigationHeaderProps> = ({
  title,
  Left,
  Right,
  viewStyle,
  titleStyle
}) => {
  return (
    <View style={[styles.view, viewStyle]}>
      <View style={[styles.left_right_view]}>{Left && Left()}</View>
      <View style={[styles.flex]}>
        <Text style={[styles.title, titleStyle]}>{title}</Text>
      </View>
      <View style={[styles.left_right_view]}>{Right && Right()}</View>
    </View>
  )
}
