import React from 'react'
import type {FC} from 'react'
import {StyleSheet, Modal, View} from 'react-native'
import * as Progress from 'react-native-progress'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    position: 'absolute',
    width: '100%',
    height: '100%',
    alignItems: 'center',
    justifyContent: 'center'
  }
})

// 로딩 컴포넌트 정의
export const Loading: FC = () => {
  return (
    <Modal transparent={true}>
      <View style={[styles.view]}>
        <Progress.CircleSnail
          color={['#E6135A']}
          size={40}
          duration={1000}
          spinDuration={4000}
          thickness={4}
        />
      </View>
    </Modal>
  )
}
