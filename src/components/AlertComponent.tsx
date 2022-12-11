import React from 'react'
import type {FC} from 'react'
import {StyleSheet, View, Text, Dimensions, Modal} from 'react-native'
import {TouchableView} from '../components'
import Color from 'color'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    position: 'absolute',
    width: '100%',
    height: '100%',
    backgroundColor: Color('#000000').alpha(0.45).string(),
    justifyContent: 'center'
  },
  alertBox: {
    backgroundColor: 'white',
    width: Dimensions.get('window').width - 80,
    marginLeft: 40,
    height: Dimensions.get('window').height / 4.5,
    borderRadius: 10,
    padding: 10
  },
  messageView: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center'
  },
  message: {
    fontSize: 15,
    color: 'black'
  },
  buttonView: {
    height: 35,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center'
  },
  okButton: {
    width: (Dimensions.get('window').width - 90) / 2 - 5,
    height: 35,
    backgroundColor: '#E6135A',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 5
  },
  okText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold'
  },
  cancelButton: {
    width: (Dimensions.get('window').width - 90) / 2 - 5,
    height: 35,
    backgroundColor: '#F1F1F5',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 5,
    marginRight: 5
  },
  cancelText: {
    color: '#999999',
    fontSize: 14,
    fontWeight: 'bold'
  }
})

// 알림 컴포넌트 매개변수 타입 정의
export type AlertComponentProps = {
  message: string // 알림 메세지 내용 매개변수
  cancelUse: boolean // 취소 버튼 사용 여부 매개변수
  okPress: () => void // 확인 버튼 클릭 이벤트 함수
  cancelPress?: () => void // 취소 버튼 클릭 이벤트 함수
}

// 알림 컴포넌트 정의
export const AlertComponent: FC<AlertComponentProps> = ({
  message,
  cancelUse,
  okPress,
  cancelPress
}) => {
  return (
    <Modal transparent={true}>
      <View
        style={{
          flex: 1,
          justifyContent: 'center',
          backgroundColor: Color('#000000').alpha(0.45).string()
        }}>
        {/* 알림 메세지 박스 */}
        <View style={[styles.alertBox]}>
          {/* 알림 메세지 */}
          <View style={[styles.messageView]}>
            <Text style={[styles.message]}>{message}</Text>
          </View>
          {/* 알림 버튼 */}
          <View style={[styles.buttonView]}>
            {/* 취소 버튼 */}
            {cancelUse && (
              <TouchableView
                style={[styles.cancelButton]}
                onPress={cancelPress}>
                <Text style={[styles.cancelText]}>취소</Text>
              </TouchableView>
            )}
            {/* 확인 버튼 */}
            <TouchableView style={[styles.okButton]} onPress={okPress}>
              <Text style={[styles.okText]}>확인</Text>
            </TouchableView>
          </View>
        </View>
      </View>
    </Modal>
  )
}
