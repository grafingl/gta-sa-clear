  �
CopsFaster�
cleo\CopsFaster.iniSettingsSpeed   �  V M ����   �    M �����   � ߀  9  
�  M �����    �   �      A�      @ ! �)
 M ����� 9  �9  �M �����           ��   r   ��   ��� ��      !  �    �      ���>��L�   �      333?  �?  	 �      ���>   @
   �      333?  �@   �      �9� � 9� � ��     	   �
        M ����� 9  �9  �M �����  �      �    climb_standM U��� climb_stand �  !  fff?M U����  �  ! ��  9  � M )����   !  �    swat_runM �����  swat_run  ���� ����FLAG   SRC   {$CLEO .cs}
{$USE CLEO+}
{$USE newOpcodes}
{$USE ini}

0000: NOP 

script_name "CopsFaster" 
0AF2: 2@ = get_float_from_ini_file "cleo\CopsFaster.ini" section "Settings" key "Speed" 

While true
wait 0 
    if
        Player.Defined($PLAYER_CHAR)
    then
        31@ = 0
        while 0EA7: get_any_char_no_save_recursive 31@ progress_to 31@ char_to 0@
            089F: get_actor 0@ pedtype_to 1@
            if and
                80DF:   not actor 0@ driving
                1@ == 6 // pedtype true cop
                not is_char_script_controlled 0@
            then

                //������ ����� �� �������
                0EFF: get_char_simplest_active_task 0@ id_to 1@ pointer_to 3@
                if and
                    010F:   player $PLAYER_CHAR wanted_level > 0
                    0EE4: locate_char_distance_to_char 0@ char $PLAYER_ACTOR radius 10.0
                    not locate_char_distance_to_char 0@ char $PLAYER_ACTOR radius 2.0
                    0019:   TIMERB > 2000
                    0A29:   player $PLAYER_CHAR climbing
                then
                    if or
                        1@ == 900 // walk, run to point
                        1@ == 1019 // fight without firegun, 1020 firegun
                    then
                        04C4: store_coords_to 4@ 5@ 6@ from_actor $PLAYER_ACTOR with_offset 0.0 0.0 -1.0
                        0172: 1@ = actor $PLAYER_ACTOR Z_angle
                        0615: define_AS_pack_begin 3@
                        05D3: AS_actor -1 goto_point 4@ 5@ 6@ mode 7 time -1 ms // versionA
                        05D4: AS_actor -1 rotate_angle 1@
                        078F: AS_actor -1 climb 1
                        0616: define_AS_pack_end 3@
                        0618: assign_actor 0@ to_AS_pack 3@
                        061B: remove_references_to_AS_pack 3@
                        0006: TIMERB = 0
                    end
                end

                // ������ ����� ���� ������� �����
                0EFF: get_char_simplest_active_task 0@ id_to 1@ pointer_to 3@
                04C4: store_coords_to 4@ 5@ 6@ from_actor 0@ with_offset 0.0 0.4 -0.2 // solid
                04C4: store_coords_to 7@ 8@ 9@ from_actor 0@ with_offset 0.0 0.7 1.0 // solid
                04C4: store_coords_to 10@ 11@ 12@ from_actor 0@ with_offset 0.0 0.4 2.0 // NOT solid
                04C4: store_coords_to 13@ 14@ 15@ from_actor 0@ with_offset 0.0 0.7 4.0 // NOT solid
                if and
                    010F:   player $PLAYER_CHAR wanted_level > 0
                    0019:   TIMERA > 2000
                    not 1@ == 210 // simple jump
                    not 1@ == 254 // simple climb
                    86BD:   not no_obstacles_between 4@ 5@ 6@ and 7@ 8@ 9@ solid 1 car 1 actor 0 object 1 particle 0 // if solid
                    06BD:   no_obstacles_between 10@ 11@ 12@ and 13@ 14@ 15@ solid 1 car 1 actor 0 object 1 particle 0 // if NOT solid
                then
                    if or
                        1@ == 900 // walk, run to point
                        1@ == 1019 // fight without firegun, 1020 firegun
                    then
                        0687: clear_actor 0@ task
                        078F: AS_actor 0@ climb 1
                        0006: TIMERA = 0
                    end
                end

                //��������� ������, ����� ���� ����� ������ �� �����, ���� ������� � ������� ������
                if
                    Actor.Animation(0@) == "climb_stand" // animation finish climb
                then
                    0613: 3@ = actor $PLAYER_ACTOR animation "climb_stand" time
                    if
                        3@ > 0.9
                    then
                        0792: disembark_instantly_actor 0@
                    end
                end

                // ���� ��� ��� ���� �����
                if and
                    0019:   TIMERB > 500
                    8118:   not actor 0@ dead
                    1@ == 203 // task stand_still
                then
                    0792: disembark_instantly_actor 0@
                    0006: TIMERB = 0
                end

                // ������� ��� �� ���� ��������� ��������
                if
                    Actor.Animation(0@) == "swat_run" // cop run animation
                then
                    0393: actor 0@ perform_animation "swat_run" at 2@ times_normal_rate // set speed from .ini
                end

            end
        end
    end
end

  __SBFTR 